using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace _PROJECT_.GP.Scripts.HUD
{
    /// <summary>
    /// Displays and toggles informational text on the HUD.
    /// </summary>
    public class InfoManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TextMeshProUGUI _infoDebugText;

        [SerializeField] private Transform _taskContent;
        [SerializeField] private GameObject _taskPrefab;

        [Header("Information")]
        [SerializeField, TextArea] private string _infoDebug;
        [SerializeField] private List<TaskUI> _tasksInQueue;

        private bool _isActive;

        private void Awake()
        {
            UpdateTextDebugInfo();
        }

        private void OnEnable()
        {
            GameManager.Instance._taskManager.OnTaskAssigned += AddTask;
            GameManager.Instance._taskManager.OnTaskCompleted += RemoveTask;
        }
        private void OnDisable()
        {
            GameManager.Instance._taskManager.OnTaskAssigned -= AddTask;
            GameManager.Instance._taskManager.OnTaskCompleted -= RemoveTask;
        }

        public void SwitchInfoDebugState()
        {
            _isActive = !_isActive;
            _infoDebugText.DOKill();
            int a = _isActive ? 0 : 1;

            _infoDebugText.DOFade(a, 0.25f);
        }
        private void UpdateTextDebugInfo()
        {
            _infoDebugText.text = _infoDebug;
        }

        private void AddTask(string taskName, string taskDescription, string taskId)
        {
            GameObject currentTaskObject = Instantiate(_taskPrefab, _taskContent);
            if (!currentTaskObject.TryGetComponent(out TaskUI taskUI)) return;

            taskUI.SetTaskInfo(taskName, taskDescription, taskId);
            _tasksInQueue.Add(taskUI);
        }

        private void RemoveTask(string taskId)
        {
            TaskUI taskToRemove = _tasksInQueue.Find(task => task.TaskID == taskId);
            if (taskToRemove == null) return;

            _tasksInQueue.Remove(taskToRemove);
            Destroy(taskToRemove.gameObject);
        }
    }
}
