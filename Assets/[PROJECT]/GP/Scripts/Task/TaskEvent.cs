using System;
using UnityEngine;
using _PROJECT_.GP.Scripts.Interactables;

namespace _PROJECT_.GP.Scripts.Task
{
    public class TaskEvent : InteractableBase
    {
        public static event Action<string> OnTaskCompleted;

        public int _taskCompleted;

        public GameObject _objectToSpawn;
        public GameObject _newObjectWhenInteracted;

        public bool _isCompleted;
        private string _currentIdTask;
        public string CurrentIdTask => _currentIdTask;

        public void Initialize(InteractionType interactionType, float holdDuration, int spamCount, GameObject objectToSpawn, GameObject newObjectWhenInteracted, string currentIdTask)
        {
            _interactionType = interactionType;
            _holdDuration = holdDuration;
            _spamCount = spamCount;
            _objectToSpawn = objectToSpawn;
            _newObjectWhenInteracted = newObjectWhenInteracted;
            _currentIdTask = currentIdTask;
            _isCompleted = false;
            CanInteract = true;
            _taskCompleted = 0;
        }

        public override void InteractIn()
        {
            // Optional
        }

        public override void InteractOut()
        {
            // Optional
        }

        public override void Interact()
        {
            if (_isCompleted) return;
            _isCompleted = true;
            _taskCompleted++;
            CanInteract = false;
            Debug.Log("Interacted with " + gameObject.name + " " + _taskCompleted);

            if (_objectToSpawn != null)
            {
                _objectToSpawn.SetActive(false);
            }
            if (_newObjectWhenInteracted != null)
            {
                _newObjectWhenInteracted.SetActive(true);
            }

            OnTaskCompleted?.Invoke(_currentIdTask);
            InteractOut();
        }
    }
}