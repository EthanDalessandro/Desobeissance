using System;
using UnityEngine;
using UnityEngine.InputSystem;
using _PROJECT_.GP.Scripts.Interactables;

namespace _PROJECT_.GP.Scripts.Player
{
    /// <summary>
    /// Optimized PlayerInteractorManager.
    /// Manages player interactions including Simple, Hold, and Spam types.
    /// Migrates physics queries to Update loop for visual sync.
    /// Implements caching to minimize GetComponent overhead.
    /// Prevents event spam through strict state checking.
    /// </summary>
    public class PlayerInteractorManager : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private Transform _originOfCast;
        [SerializeField] private float _interactionDistance = 3.0f;
        [SerializeField] private float _interactionRadius = 0.5f;
        [SerializeField] private LayerMask _hitLayers;

        private RaycastHit _hitInfo;
        private IInteractable _currentInteractable;
        private Collider _lastHitCollider;

        // Debug
        private bool _debugIsHit;
        private Vector3 _debugHitPosition;

        // Events
        public event Action OnInteractIn;
        public event Action OnInteractOut;
        public event Action OnInteract;

        // State
        private float _currentHoldTime;
        private int _currentSpamCount;
        private bool _isHolding;

        private void Update()
        {
            if (_currentInteractable != null && _currentInteractable.CanInteract == false)
            {
                // Force exit if current interactable is no longer available
                _currentInteractable.InteractOut();
                OnInteractOut?.Invoke();
                _currentInteractable = null;
                _lastHitCollider = null;
                return;
            }
            PerformInteractionCheck();
            HandleHoldInteraction();
        }

        #region Interaction Detection

        private void PerformInteractionCheck()
        {
            if (GameManager.Instance._gameState == GameState.Intimidation) return;

            IInteractable detectedInteractable = null;
            Collider hitCollider = null;

            if (TryDetectInteractable(out detectedInteractable, out hitCollider))
            {
                // Detection successful
            }

            if (hitCollider != _lastHitCollider || detectedInteractable != _currentInteractable)
            {
                UpdateInteractionState(detectedInteractable, hitCollider);
            }
        }

        private bool TryDetectInteractable(out IInteractable interactable, out Collider collider)
        {
            interactable = null;
            collider = null;

            bool isHit = Physics.SphereCast(_originOfCast.position, _interactionRadius, _originOfCast.forward, out _hitInfo, _interactionDistance, _hitLayers);
            _debugIsHit = isHit;

            if (isHit)
            {
                _debugHitPosition = _hitInfo.point;
                collider = _hitInfo.collider;

                if (collider == _lastHitCollider)
                {
                    interactable = _currentInteractable;
                }
                else
                {
                    collider.TryGetComponent(out interactable);
                }

                // Reject interactables that cannot be interacted with
                if (interactable != null && !interactable.CanInteract)
                {
                    interactable = null;
                    collider = null;
                    return false;
                }

                return true;
            }

            return false;
        }

        private void UpdateInteractionState(IInteractable newInteractable, Collider newCollider)
        {
            // Exit previous interaction
            if (_currentInteractable != null && newInteractable != _currentInteractable)
            {
                _currentInteractable.InteractOut();
                OnInteractOut?.Invoke();
            }

            // Enter new interaction
            if (newInteractable != null && newInteractable != _currentInteractable)
            {
                newInteractable.InteractIn();
                OnInteractIn?.Invoke();
            }

            _currentInteractable = newInteractable;
            _lastHitCollider = newCollider;
        }

        #endregion

        #region Interaction Handling

        public void Interact(InputAction.CallbackContext context)
        {
            if (_currentInteractable == null) return;

            switch (_currentInteractable.InteractionType)
            {
                case InteractionType.Simple:
                    HandleSimpleInteraction(context);
                    break;
                case InteractionType.Hold:
                    HandleHoldInput(context);
                    break;
                case InteractionType.Spam:
                    HandleSpamInteraction(context);
                    break;
            }
        }

        private void HandleSimpleInteraction(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                ExecuteInteraction();
            }
        }

        private void HandleHoldInput(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                _isHolding = true;
                _currentHoldTime = 0f;
            }
            else if (context.canceled)
            {
                _isHolding = false;
                _currentHoldTime = 0f;
            }
        }

        private void HandleHoldInteraction()
        {
            if (_currentInteractable == null || _currentInteractable.InteractionType != InteractionType.Hold)
            {
                if (_isHolding)
                {
                    _isHolding = false;
                    _currentHoldTime = 0f;
                }
                return;
            }

            if (_isHolding)
            {
                _currentHoldTime += Time.deltaTime;
                if (_currentHoldTime >= _currentInteractable.HoldDuration)
                {
                    ExecuteInteraction();
                    _isHolding = false;
                    _currentHoldTime = 0f;
                }
            }
        }

        private void HandleSpamInteraction(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _currentSpamCount++;
                if (_currentSpamCount >= _currentInteractable.SpamCount)
                {
                    ExecuteInteraction();
                    _currentSpamCount = 0;
                }
            }
        }

        private void ExecuteInteraction()
        {
            _currentInteractable?.Interact();
            OnInteract?.Invoke();
        }

        #endregion

        #region Debug

        private void OnDrawGizmos()
        {
            if (_originOfCast == null) return;

            bool isHit = Physics.SphereCast(_originOfCast.position, _interactionRadius, _originOfCast.forward, out RaycastHit gizmoHit, _interactionDistance, _hitLayers);

            float distanceToDraw = isHit ? gizmoHit.distance : _interactionDistance;
            Gizmos.color = isHit ? Color.red : Color.green;

            Vector3 originPos = _originOfCast.position;
            Vector3 endPos = _originOfCast.position + (_originOfCast.forward * distanceToDraw);

            DrawCapsuleGizmo(originPos, endPos, _interactionRadius);
        }

        private void DrawCapsuleGizmo(Vector3 start, Vector3 end, float radius)
        {
            Gizmos.DrawWireSphere(start, radius);
            Gizmos.DrawWireSphere(end, radius);
            Gizmos.DrawLine(start, end);

            Vector3 up = _originOfCast.up * radius;
            Vector3 right = _originOfCast.right * radius;

            Gizmos.DrawLine(start + up, end + up);
            Gizmos.DrawLine(start - up, end - up);
            Gizmos.DrawLine(start + right, end + right);
            Gizmos.DrawLine(start - right, end - right);
        }

        #endregion
    }
}