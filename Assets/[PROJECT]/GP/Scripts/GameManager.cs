using _PROJECT_.GP.Scripts.HUD;
using _PROJECT_.GP.Scripts.Player;
using UnityEngine;

namespace _PROJECT_.GP.Scripts
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;
        
        [Header("References")]
        public PlayerInteractorManager  _playerInteractorManager;
        public CrossHairsManager _crossHairsManager;

        private void Awake()
        {
            Instance = this;
        }
    }
}
