using System;
using UnityEngine;

namespace _PROJECT_.GP.Scripts.HUD
{
    public class CrossHairsManager : MonoBehaviour
    {
        [SerializeField] private GameObject _mainCrossHair;
        [SerializeField] private GameObject _handCrossHair;

        public event Action OnHoverEnter;
        public event Action OnHoverExit;
        private void Awake()
        {
            ShowMainCrosshair();
        }

        private void ShowHandCrosshair()
        {
            HideAll();
            _handCrossHair.SetActive(true);
            OnHoverEnter?.Invoke();
        }
        
        private void ShowMainCrosshair()
        {
            HideAll();
            _mainCrossHair.SetActive(true);
            OnHoverExit?.Invoke();
        }

        private void HideAll()
        {
            _mainCrossHair.SetActive(false);
            _handCrossHair.SetActive(false);
        }
    }
}
