using UnityEngine;
using System;
using System.Collections.Generic;
using _PROJECT_.GP.Scripts.ScriptablesObjects;
using _PROJECT_.GP.Scripts.Interactables;
using Random = UnityEngine.Random;


namespace _PROJECT_.GP.Scripts.Task
{
    [System.Serializable]
    public class TaskEntry
    {
        public TaskList taskData;
        public TaskEvent taskEvent;
    }

    public class TaskManager : MonoBehaviour
    {
        [SerializeField] private int _taskLeft;
        [SerializeField] private List<TaskEntry> _tasks;

        private GameManager _gameManager;
        private bool _canAssignTask = true;

        public event Action<string, string, string> OnTaskAssigned;
        public event Action<string> OnTaskCompleted;

        public void Initialize(GameManager gameManager)
        {
            _gameManager = gameManager;
        }

        private void OnEnable()
        {
            TaskEvent.OnTaskCompleted += HandleTaskCompleted;
        }

        private void OnDisable()
        {
            TaskEvent.OnTaskCompleted -= HandleTaskCompleted;
        }

        private void HandleTaskCompleted(string taskId)
        {
            _canAssignTask = true;
            OnTaskCompleted?.Invoke(taskId);
            Debug.Log($"Task {taskId} completed. Can assign new task.");
        }

        private void Start()
        {
            AssignTask();
        }

        /// <summary>
        /// Attempts to assign a new task to the player.
        /// It first checks if tasks can be assigned and if there are tasks remaining.
        /// It then tries to find an uncompleted task from the available list, initializing it with specific interaction details.
        /// Finally, it decrements the total tasks left, prevents immediate re-assignment, and notifies subscribers that a task has been assigned.
        /// </summary>
        public void AssignTask()
        {
            if (!_canAssignTask || _taskLeft <= 0) return;

            int attempts = 0;
            const int maxAttempts = 25;

            while (attempts < maxAttempts)
            {
                int randomIndex = Random.Range(0, _tasks.Count);

                TaskEntry entry = _tasks[randomIndex];
                TaskEvent taskEvent = entry.taskEvent;
                TaskList taskData = entry.taskData;

                // Check if task is valid (not completed and has remaining steps)
                if (!taskEvent._isCompleted && taskEvent._taskCompleted < taskData.tasks.Count)
                {
                    // Get current step info
                    TaskInfo currentStepInfo = taskData.tasks[taskEvent._taskCompleted];
                    string currentStepId = "id" + Random.Range(0, 100000);

                    // Initialize task event
                    taskEvent.Initialize(
                        currentStepInfo.interactionType,
                        currentStepInfo.holdDuration,
                        currentStepInfo.spamCount,
                        currentStepInfo.objectToSpawn,
                        currentStepInfo.newObjectWhenInteracted,
                        currentStepId
                    );

                    // Finalize assignment
                    _taskLeft--;
                    //_canAssignTask = false;
                    OnTaskAssigned?.Invoke(currentStepInfo.name, currentStepInfo.taskDescription, currentStepId);
                    return;
                }
                attempts++;
            }
        }

    }
}