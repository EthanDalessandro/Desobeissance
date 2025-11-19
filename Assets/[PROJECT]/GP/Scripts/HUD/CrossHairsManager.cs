using System;
using UnityEngine;

namespace _PROJECT_.GP.Scripts.HUD
{
    public class CrossHairsManager : MonoBehaviour
    {
        [SerializeField] private GameObject _mainCrossHair;
        [SerializeField] private GameObject _handCrossHair;
        private void Awake()
        {
            ShowMainCrosshair();
        }

        private void OnEnable()
        {
            GameManager.Instance._playerInteractorManager.OnInteractIn += ShowHandCrosshair;
            GameManager.Instance._playerInteractorManager.OnInteractOut += ShowMainCrosshair;
        }

        private void OnDisable()
        {
            GameManager.Instance._playerInteractorManager.OnInteractIn -= ShowHandCrosshair;
            GameManager.Instance._playerInteractorManager.OnInteractOut -= ShowMainCrosshair;
        }

        private void ShowHandCrosshair()
        {
            print("ShowHandCrosshair");
            HideAll();
            _handCrossHair.SetActive(true);
        }
        
        private void ShowMainCrosshair()
        {
            print("ShowMainCrosshair");
            HideAll();
            _mainCrossHair.SetActive(true);
        }

        private void HideAll()
        {
            _mainCrossHair.SetActive(false);
            _handCrossHair.SetActive(false);
        }
    }
}
