using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _PROJECT_.GP.Scripts.Player
{
    public class PlayerController : MonoBehaviour
    {
        private Rigidbody _rb;

        private Vector2 _moveInput;

        [Header("Movement Settings")] [SerializeField, Range(1, 20)]
        private float _moveSpeed;

        [SerializeField, Range(0, 50)] private float _groundFriction;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _rb.linearDamping = _groundFriction;
        }

        private void FixedUpdate()
        {
            if (_moveInput == Vector2.zero) return;

            Vector3 forward = transform.forward * _moveInput.y;
            Vector3 right = transform.right * _moveInput.x;

            Vector3 moveDirection = forward + right;

            _rb.linearVelocity = moveDirection * _moveSpeed;
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            _moveInput = context.ReadValue<Vector2>();
        }
    }
}