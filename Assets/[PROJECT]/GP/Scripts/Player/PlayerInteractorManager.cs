using System;
using System.Collections.Generic;
using _PROJECT_.GP.Scripts.Interactables;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _PROJECT_.GP.Scripts.Player
{
    public class PlayerInteractorManager : MonoBehaviour
    {
        [SerializeField] private Transform _originOfCast;
        private RaycastHit _hitInfo;
        private IInteractable _interactable;
        
        [Header("Interactor Settings")]
        [SerializeField] private float _interactionDistance;
        [SerializeField] private float _interactionRadius;
        [SerializeField] private LayerMask _hitLayers;

        private void FixedUpdate()
        {
            IInteractable newInteractable = null;

            if (Physics.SphereCast(_originOfCast.position, _interactionRadius, _originOfCast.forward, out _hitInfo, _interactionDistance, _hitLayers))
            {
                newInteractable = _hitInfo.collider.GetComponent<IInteractable>();
            }
            if (_interactable == newInteractable) return;

            UpdateInteractable(_interactable, newInteractable);
        }

        private void UpdateInteractable(IInteractable previousOne, IInteractable newOne)
        {
            previousOne?.InteractOut();
            newOne?.InteractIn();
            _interactable = newOne;
        }

        public void Interact(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _interactable?.Interact();
            }
        }
        
        private void OnDrawGizmos()
        {
            if (_originOfCast == null) return;

            bool isHit = Physics.SphereCast(
                _originOfCast.position, 
                _interactionRadius, 
                _originOfCast.forward, 
                out RaycastHit gizmoHit, 
                _interactionDistance, 
                _hitLayers
            );

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