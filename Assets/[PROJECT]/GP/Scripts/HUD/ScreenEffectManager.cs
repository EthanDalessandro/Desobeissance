using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _PROJECT_.GP.Scripts.HUD
{
    public class ScreenEffectManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Image _darkScreen;

        [Header("Settings")]
        [SerializeField] private float _screenFadeDuration = 0.25f;
        [SerializeField] private float _pauseTime = 0.25f;

        private void OnEnable()
        {
            GameManager.Instance.OnSceneReset += DarkScreenEffect;
        }
        private void OnDisable()
        {
            GameManager.Instance.OnSceneReset -= DarkScreenEffect;
        }

        private void DarkScreenEffect()
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(_darkScreen.DOFade(1, _screenFadeDuration));
            sequence.AppendInterval(_pauseTime);
            sequence.Append(_darkScreen.DOFade(0, _screenFadeDuration));
        }
    }
}
