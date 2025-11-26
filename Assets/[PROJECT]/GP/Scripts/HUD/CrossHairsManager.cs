using System;
using UnityEngine;

namespace _PROJECT_.GP.Scripts.HUD
{
    /// <summary>
    /// Controls the visibility and state of different crosshair types (Main, Hand, Eyes).
    /// </summary>
    public class CrossHairsManager : MonoBehaviour
    {
        [SerializeField] private GameObject _mainCrossHair;
        [SerializeField] private GameObject _handCrossHair;
        [SerializeField] private GameObject _eyesCrossHair;
        public void ShowHandCrosshair()
        {
            HideAll();
            _handCrossHair.SetActive(true);
        }

        public void ShowMainCrosshair()
        {
            HideAll();
            _mainCrossHair.SetActive(true);
        }

        public void ShowEyesCrossHair()
        {
            HideAll();
            _eyesCrossHair.SetActive(true);
        }

        private void HideAll()
        {
            _mainCrossHair.SetActive(false);
            _handCrossHair.SetActive(false);
            _eyesCrossHair.SetActive(false);
        }
    }
}
