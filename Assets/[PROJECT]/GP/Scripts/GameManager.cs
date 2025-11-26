using System;
using System.Collections.Generic;
using _PROJECT_.GP.Scripts.HUD;
using _PROJECT_.GP.Scripts.Player;
using _PROJECT_.GP.Scripts.Task;
using _PROJECT_.GP.Scripts.Interactables;
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
    /// Acts as the central hub for game logic, event distribution, and dependency instantiation.
    /// </summary>
    [DefaultExecutionOrder(-100)]
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        [Header("Prefabs")]
        [SerializeField] private GameObject _playerPrefab;
        [SerializeField] private GameObject _hudPrefab;
        [SerializeField] private GameObject _taskManagerPrefab;

        [Header("References")]
        public PlayerInteractorManager _playerInteractorManager;
        public PlayerCameraController _playerCameraController;
        public HudManager _hudManager;
        public TaskManager _taskManager;

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

            // Validate essential prefabs
            if (!_playerPrefab) Debug.LogError("Player Prefab is not assigned in GameManager.");
            if (!_hudPrefab) Debug.LogError("HUD Prefab is not assigned in GameManager.");
            if (!_taskManagerPrefab) Debug.LogError("Task Manager Prefab is not assigned in GameManager.");

            // Ensure Player is present in the scene, instantiate if not
            EnsurePlayer();

            // Ensure HUD is present in the scene, instantiate if not
            EnsureHud();

            // Ensure TaskManager is present in the scene, instantiate if not
            EnsureTaskManager();

            // Initialize all managers
            _hudManager?.Initialize(this);
            _playerCameraController?.Initialize(this);
            _taskManager?.Initialize(this);

            // Final UI setup
            _hudManager?._crossHairsManager?.ShowMainCrosshair();
        }

        private void EnsurePlayer()
        {
            if (_playerInteractorManager == null || _playerCameraController == null)
            {
                if (_playerPrefab != null)
                {
                    Debug.LogWarning("Player not found in scene, instantiating from prefab.");
                    GameObject player = Instantiate(_playerPrefab, transform.position, Quaternion.identity);
                    _playerInteractorManager = player.GetComponent<PlayerInteractorManager>();
                    _playerCameraController = player.GetComponent<PlayerCameraController>();
                }
                else
                {
                    Debug.LogError("Player prefab is missing, cannot instantiate player.");
                }
            }
        }

        private void EnsureHud()
        {
            if (_hudManager == null)
            {
                if (_hudPrefab != null)
                {
                    Debug.LogWarning("HUD not found in scene, instantiating from prefab.");
                    GameObject hud = Instantiate(_hudPrefab);
                    _hudManager = hud.GetComponent<HudManager>();
                }
                else
                {
                    Debug.LogError("HUD prefab is missing, cannot instantiate HUD.");
                }
            }
        }

        private void EnsureTaskManager()
        {
            if (_taskManager == null)
            {
                if (_taskManagerPrefab != null)
                {
                    Debug.LogWarning("Task Manager not found in scene, instantiating from prefab.");
                    GameObject taskManager = Instantiate(_taskManagerPrefab);
                    _taskManager = taskManager.GetComponent<TaskManager>();
                }
                else
                {
                    Debug.LogError("Task Manager prefab is missing, cannot instantiate Task Manager.");
                }
            }
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
