using System;
using System.Collections.Generic;
using _PROJECT_.GP.Scripts.HUD;
using _PROJECT_.GP.Scripts.Player;
using UnityEngine;

namespace _PROJECT_.GP.Scripts
{
    public enum GameState
    {
        Normal,
        Intimidation,
        FreeTime
    }
    /// <summary>
    /// Manages the global game state and coordinates interactions between major systems.
    /// Acts as the central hub for game logic and event distribution.
    /// </summary>
    [DefaultExecutionOrder(-100)]
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        [Header("Prefabs")]
        [SerializeField] private GameObject _playerPrefab;
        [SerializeField] private GameObject _hudPrefab;

        [Header("References")]
        public PlayerInteractorManager _playerInteractorManager;
        public PlayerCameraController _playerCameraController;
        public HudManager _hudManager;

        private Dictionary<GameState, Action> _gameStateEvents;

        [Header("Game State")]
        public GameState _gameState;

        //Event Scene
        public event Action OnSceneSwitch;

        //Event Player
        public Action<bool> OnIntimidationTriggered;

        //Event Interaction

        private void Awake()
        {
            Instance = this;
        
            InitializeEvents();

            if (!_playerPrefab || !_hudPrefab)
            {
                Debug.LogError("Player or HUD prefab is not assigned.");
                return;
            }
            if(!_playerInteractorManager || !_playerCameraController)
            {
                Debug.LogWarning("Player Instantiated cause there was no reference in scene assigned.");
                GameObject player = Instantiate(_playerPrefab, transform.position, Quaternion.identity);
                _playerInteractorManager = player.GetComponent<PlayerInteractorManager>();
                _playerCameraController = player.GetComponent<PlayerCameraController>();
            }
            if(!_hudManager)
            {
                Debug.LogWarning("HUD Instantiated cause there was no reference in scene assigned.");
                GameObject hud = Instantiate(_hudPrefab);
                _hudManager = hud.GetComponent<HudManager>();
            }

            _hudManager.Initialize(this);
            if (_playerCameraController != null)
            {
                _playerCameraController.Initialize(this);
            }

            _hudManager._crossHairsManager.ShowMainCrosshair();
        }

        private void InitializeEvents()
        {
            _gameStateEvents = new Dictionary<GameState, Action>();

            foreach (GameState state in Enum.GetValues(typeof(GameState)))
            {
                _gameStateEvents.Add(state, null);
            }
        }
        public void RegisterOnStateEnter(GameState state, Action callback)
        {
            if (_gameStateEvents.ContainsKey(state))
            {
                _gameStateEvents[state] += callback;
            }
        }
        public void UnregisterOnStateEnter(GameState state, Action callback)
        {
            if (_gameStateEvents.ContainsKey(state))
            {
                _gameStateEvents[state] -= callback;
            }
        }
        /*// Je m'abonne : "Quand le jeu passe en Intimidation, appelle ma fonction BecomeAggressive"
        GameManager.Instance.RegisterOnStateEnter(GameState.Intimidation, BecomeAggressive);
        
        // Je m'abonne : "Quand le jeu passe en Normal, appelle ma fonction CalmDown"
        GameManager.Instance.RegisterOnStateEnter(GameState.Normal, CalmDown);

        // TOUJOURS se désabonner quand l'objet est détruit pour éviter les erreurs

        GameManager.Instance.UnregisterOnStateEnter(GameState.Intimidation, BecomeAggressive);
        GameManager.Instance.UnregisterOnStateEnter(GameState.Normal, CalmDown);*/
        public void SwitchGameState(GameState newGameState)
        {
            _gameState = newGameState;
            _gameStateEvents[newGameState]?.Invoke();
            OnIntimidationTriggered?.Invoke(newGameState == GameState.Intimidation);
        }
        public void ResetScene()
        {
            OnSceneSwitch?.Invoke();
        }
    }
}
