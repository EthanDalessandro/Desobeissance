using System.Collections.Generic;
using UnityEngine;
using _PROJECT_.GP.Scripts.Interactables;

namespace _PROJECT_.GP.Scripts.ScriptablesObjects
{
    [System.Serializable]
    public class TaskInfo
    {
        public int taskIndex;
        public string name;
        public string taskDescription;
        public GameObject objectToSpawn;
        public GameObject newObjectWhenInteracted;

        public InteractionType interactionType;
        public float holdDuration;
        public int spamCount;
    }

    [CreateAssetMenu(fileName = "New Task List", menuName = "ScriptableObjects/TaskList")]
    public class TaskList : ScriptableObject
    {
        public List<TaskInfo> tasks;
    }
}
