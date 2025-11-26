using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _PROJECT_.GP.Scripts.Player
{
    /// <summary>
    /// Controls camera rotation and head movement.
    /// Detects if the player is looking down to trigger intimidation effects.
    /// </summary>
    public class PlayerCameraController : MonoBehaviour
    {
        private GameManager _gameManager;
        private Transform _body;
        [SerializeField] private Transform _head;

        [Header("Mouse Settings")]
        [SerializeField, Range(1, 10)]
        private float _rotateSpeedX;

        [SerializeField, Range(1, 10)] private float _rotateSpeedY;
        [SerializeField] private Vector2 _headRotationLimit;
        [SerializeField] private float _headDownOrderedLimit;
        private Vector2 _moveInput;

        private float _xRotation = 0f;

        private void Awake()
        {
            _body = transform;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        bool _isIntimidationTriggered = false;

        private void LateUpdate()
        {
            HeadDownCheck();
            
            if (_moveInput.sqrMagnitude >= 0.01f)
            {
                float mouseX = _moveInput.x * _rotateSpeedX / 100;
                float mouseY = _moveInput.y * _rotateSpeedY / 100;

                _xRotation -= mouseY;

                _xRotation = Mathf.Clamp(_xRotation, _headRotationLimit.x, _headRotationLimit.y);

                _head.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);

                _body.Rotate(Vector3.up * mouseX);
            }

            _moveInput = Vector2.zero;
        }

        public void OnMouseMove(InputAction.CallbackContext context)
        {
            _moveInput += context.ReadValue<Vector2>();
        }

        public void Initialize(GameManager gameManager)
        {
            _gameManager = gameManager;
        }

        private void HeadDownCheck()
        {
            if (!_gameManager || _gameManager._gameState != GameState.Intimidation) return;

            // If rotation is LESS than limit (looking up/forward) -> Trigger Effect
            if (_xRotation < _headDownOrderedLimit)
            {
                if (_isIntimidationTriggered) return;

                _isIntimidationTriggered = true;
            }
            // If rotation is GREATER OR EQUAL (looking down) -> Stop Effect
            else
            {
                if (!_isIntimidationTriggered) return;

                _isIntimidationTriggered = false;
            }

            _gameManager.OnIntimidationTriggered?.Invoke(_isIntimidationTriggered);
        }
    }
}