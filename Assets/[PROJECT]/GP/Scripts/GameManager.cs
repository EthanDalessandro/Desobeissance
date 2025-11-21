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
    [DefaultExecutionOrder(-100)]
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        [Header("References")]
        public PlayerInteractorManager _playerInteractorManager;
        public HudManager _hudManager;

        private Dictionary<GameState, Action> _gameStateEvents;

        [Header("Game State")]
        public GameState _gameState;

        //Event Scene
        public event Action OnSceneReset;

        //Event Player
        public Action<bool> OnIntimidationTriggered;
        
        //Event Interaction

        private void Awake()
        {
            Instance = this;
            InitializeEvents();
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
            OnSceneReset?.Invoke();
        }
    }
}
