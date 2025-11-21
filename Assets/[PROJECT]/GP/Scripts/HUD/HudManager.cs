using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _PROJECT_.GP.Scripts.HUD
{
    public class HudManager : MonoBehaviour
    {
        public CrossHairsManager _crossHairsManager;
        public InfoTip _infoTip;
        public ScreenEffectManager _screenEffectManager;

        private void OnEnable()
        {
            if (GameManager.Instance == null) return;

            GameManager.Instance._playerInteractorManager.OnInteractIn += _crossHairsManager.ShowHandCrosshair;
            GameManager.Instance._playerInteractorManager.OnInteractOut += _crossHairsManager.ShowMainCrosshair;
            GameManager.Instance.OnIntimidationTriggered += HandleCrosshairIntimidation;

            GameManager.Instance.OnSceneReset += _screenEffectManager.DarkScreenEffect;
            GameManager.Instance.OnIntimidationTriggered += _screenEffectManager.HandleIntimidationTrigger;
        }

        private void OnDisable()
        {
            if (GameManager.Instance == null) return;

            GameManager.Instance._playerInteractorManager.OnInteractIn -= _crossHairsManager.ShowHandCrosshair;
            GameManager.Instance._playerInteractorManager.OnInteractOut -= _crossHairsManager.ShowMainCrosshair;
            GameManager.Instance.OnIntimidationTriggered -= HandleCrosshairIntimidation;

            GameManager.Instance.OnSceneReset -= _screenEffectManager.DarkScreenEffect;
            GameManager.Instance.OnIntimidationTriggered -= _screenEffectManager.HandleIntimidationTrigger;
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
