using System;
using _PROJECT_.GP.Scripts.HUD;
using _PROJECT_.GP.Scripts.Player;
using UnityEngine;

namespace _PROJECT_.GP.Scripts
{
    [DefaultExecutionOrder(-100)]
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        [Header("References")]
        public PlayerInteractorManager _playerInteractorManager;
        public HudManager _hudManager;

        //Event
        public event Action OnSceneReset;

        private void Awake()
        {
            Instance = this;
        }
        public void ResetScene()
        {
            OnSceneReset?.Invoke();
        }
    }
}
