using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _PROJECT_.GP.Scripts.HUD
{
    /// <summary>
    /// Manages the Heads-Up Display (HUD) elements.
    /// Coordinates crosshairs, info tips, and screen effects based on game events.
    /// </summary>
    public class HudManager : MonoBehaviour
    {
        public CrossHairsManager _crossHairsManager;
        public InfoTip _infoTip;
        public ScreenEffectManager _screenEffectManager;

        private GameManager _gameManager;

        public void Initialize(GameManager gameManager)
        {
            _gameManager = gameManager;
            SubscribeToEvents();
        }

        private void OnEnable()
        {
            SubscribeToEvents();
        }

        private void OnDisable()
        {
            UnsubscribeFromEvents();
        }

        private void SubscribeToEvents()
        {
            if (_gameManager == null) return;
            //CrossHair
            _gameManager._playerInteractorManager.OnInteractIn += _crossHairsManager.ShowHandCrosshair;
            _gameManager._playerInteractorManager.OnInteractOut += _crossHairsManager.ShowMainCrosshair;
            _gameManager.OnIntimidationTriggered += HandleCrosshairIntimidation;
            //ScreenEffect
            _gameManager.OnIntimidationTriggered += _screenEffectManager.HandleIntimidationTrigger;
        }

        private void UnsubscribeFromEvents()
        {
            if (_gameManager == null) return;
            //CrossHair
            _gameManager._playerInteractorManager.OnInteractIn -= _crossHairsManager.ShowHandCrosshair;
            _gameManager._playerInteractorManager.OnInteractOut -= _crossHairsManager.ShowMainCrosshair;
            _gameManager.OnIntimidationTriggered -= HandleCrosshairIntimidation;
            //ScreenEffect

            _gameManager.OnIntimidationTriggered -= _screenEffectManager.HandleIntimidationTrigger;
        }

        private void HandleCrosshairIntimidation(bool isTriggered)
        {
            if (isTriggered)
            {
                _crossHairsManager.ShowEyesCrossHair();
            }
            else
            {
                _crossHairsManager.ShowMainCrosshair();
            }
        }

        public void ShowHideInfo(InputAction.CallbackContext context)
        {
            _infoTip.SwitchInfoState();
        }
    }
}
