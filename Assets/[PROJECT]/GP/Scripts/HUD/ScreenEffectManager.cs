using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _PROJECT_.GP.Scripts.HUD
{
    /// <summary>
    /// Handles full-screen visual effects such as fades and intimidation overlays.
    /// </summary>
    public class ScreenEffectManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Image _darkScreen;
        [SerializeField] private Image _intimidationScreen;

        [Header("Settings")]
        [SerializeField] private float _darkScreenFadeDuration = 0.25f;
        [SerializeField] private float _darkScreenPauseTime = 0.25f;

        public void HandleIntimidationTrigger(bool isTriggered)
        {
            if (isTriggered)
            {
                IntimidationScreenEffect();
            }
            else
            {
                StopIntimidationScreenEffect();
            }
        }

        public void DarkScreenEffect()
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(_darkScreen.DOFade(1, _darkScreenFadeDuration));
            sequence.AppendInterval(_darkScreenPauseTime);
            sequence.Append(_darkScreen.DOFade(0, _darkScreenFadeDuration));
        }

        private void IntimidationScreenEffect()
        {
            _intimidationScreen.DOKill();
            _intimidationScreen.DOFade(0.4f, 0.3f).SetLoops(-1, LoopType.Yoyo);
        }

        private void StopIntimidationScreenEffect()
        {
            _intimidationScreen.DOKill();
            _intimidationScreen.DOFade(0, 0.1f);
        }
    }
}
