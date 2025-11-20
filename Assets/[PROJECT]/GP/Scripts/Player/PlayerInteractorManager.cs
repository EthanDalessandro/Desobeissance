using System;
using UnityEngine;
using UnityEngine.InputSystem;
using _PROJECT_.GP.Scripts.Interactables;

namespace _PROJECT_.GP.Scripts.Player
{
    /// <summary>
    /// Optimized PlayerInteractorManager.
    /// Migrates physics queries to Update loop for visual sync.
    /// Implements caching to minimize GetComponent overhead.
    /// Prevents event spam through strict state checking.
    /// </summary>
    public class PlayerInteractorManager : MonoBehaviour
    {
        [SerializeField] private Transform _originOfCast;

        [SerializeField] private float _interactionDistance = 3.0f;
        [SerializeField] private float _interactionRadius = 0.5f;
        [SerializeField] private LayerMask _hitLayers;

        private RaycastHit _hitInfo;
        private IInteractable _currentInteractable;
        private Collider _lastHitCollider;

        private bool _debugIsHit;
        private Vector3 _debugHitPosition;

        public event Action OnInteractIn;
        public event Action OnInteractOut;
        public event Action OnInteract;

        private void Update()
        {
            PerformInteractionCheck();
        }

        private void PerformInteractionCheck()
        {
            IInteractable detectedInteractable = null;
            Collider hitCollider = null;

            bool isHit = Physics.SphereCast(_originOfCast.position, _interactionRadius, _originOfCast.forward, out _hitInfo, _interactionDistance, _hitLayers);
            
            _debugIsHit = isHit;
            
            if (isHit) _debugHitPosition = _hitInfo.point;
            
            if (isHit)
            {
                hitCollider = _hitInfo.collider;
                if (hitCollider == _lastHitCollider)
                {
                    detectedInteractable = _currentInteractable;
                }
                else
                {
                    hitCollider.TryGetComponent(out detectedInteractable);
                }
            }

            if (hitCollider != _lastHitCollider || detectedInteractable != _currentInteractable)
            {
                UpdateInteractionState(detectedInteractable, hitCollider);
            }
        }
        private void UpdateInteractionState(IInteractable newInteractable, Collider newCollider)
        {
            if (_currentInteractable != null)
            {
                if (newInteractable != _currentInteractable)
                {
                    _currentInteractable.InteractOut();
                    OnInteractOut?.Invoke();
                }
            }
            if (newInteractable != null)
            {
                if (newInteractable != _currentInteractable)
                {
                    newInteractable.InteractIn();
                    OnInteractIn?.Invoke();
                }
            }
            _currentInteractable = newInteractable;
            _lastHitCollider = newCollider;
        }
        public void Interact(InputAction.CallbackContext context)
        {
            if (!context.performed) return;

            if (_currentInteractable == null) return;

            _currentInteractable.Interact();
            OnInteract?.Invoke();
        }
        private void OnDrawGizmos()
        {
            if (_originOfCast == null) return;
            
            bool isHit = Physics.SphereCast(_originOfCast.position, _interactionRadius, _originOfCast.forward, out RaycastHit gizmoHit, _interactionDistance, _hitLayers);
            
            float distanceToDraw;

            if (isHit)
            {
                Gizmos.color = Color.red;
                distanceToDraw = gizmoHit.distance;
            }
            else
            {
                Gizmos.color = Color.green;
                distanceToDraw = _interactionDistance;
            }
            Vector3 originPos = _originOfCast.position;
            Vector3 endPos = _originOfCast.position + (_originOfCast.forward * distanceToDraw);

            Gizmos.DrawWireSphere(originPos, _interactionRadius);
            Gizmos.DrawWireSphere(endPos, _interactionRadius);
            Gizmos.DrawLine(originPos, endPos);
            
            Vector3 up = _originOfCast.up * _interactionRadius;
            Vector3 right = _originOfCast.right * _interactionRadius;

            Gizmos.DrawLine(originPos + up, endPos + up);
            Gizmos.DrawLine(originPos - up, endPos - up);
            Gizmos.DrawLine(originPos + right, endPos + right);
            Gizmos.DrawLine(originPos - right, endPos - right);
        }
    }
}