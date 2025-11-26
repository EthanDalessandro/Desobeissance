using System;
using UnityEngine;
using _PROJECT_.GP.Scripts.Interactables;

namespace _PROJECT_.GP.Scripts.Task
{   
    public class TaskEvent : InteractableBase
    {
        public int _taskIndex;
        public int _taskCompleted;
        
        public bool _isCompleted;
    
        public void Initialize(InteractionType interactionType, float holdDuration, int spamCount)
        {   
            _interactionType = interactionType;
            _holdDuration = holdDuration;
            _spamCount = spamCount;

            _isCompleted = false;
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
            _isCompleted = true;
            _taskCompleted++;
            Debug.Log("Interacted with " + gameObject.name + " " + _taskCompleted + " / " + _taskIndex);
        }
    }
}