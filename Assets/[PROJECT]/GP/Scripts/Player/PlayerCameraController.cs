using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _PROJECT_.GP.Scripts.Player
{
    public class PlayerCameraController : MonoBehaviour
    {
        private Transform _body;
        [SerializeField] private Transform _head;

        [Header("Mouse Settings")] [SerializeField, Range(1, 10)]
        private float _rotateSpeedX;

        [SerializeField, Range(1, 10)] private float _rotateSpeedY;
        [SerializeField] private Vector2 _headRotationLimit;
        private Vector2 _moveInput;

        private float _xRotation = 0f;

        private void Awake()
        {
            _body = transform;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void LateUpdate()
        {
            if (_moveInput.sqrMagnitude < 0.01f) return;

            float mouseX = _moveInput.x * _rotateSpeedX / 100;
            float mouseY = _moveInput.y * _rotateSpeedY / 100;

            _xRotation -= mouseY;

            _xRotation = Mathf.Clamp(_xRotation, _headRotationLimit.x, _headRotationLimit.y);

            _head.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);

            _body.Rotate(Vector3.up * mouseX);
        }

        public void OnMouseMove(InputAction.CallbackContext context)
        {
            _moveInput = context.ReadValue<Vector2>();
        }
    }
}