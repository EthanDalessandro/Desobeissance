using TMPro;
using UnityEngine;
using DG.Tweening;

public class TaskUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _taskTitle;
    [SerializeField] private TextMeshProUGUI _taskDescription;

    [SerializeField] private GameObject _taskCheck;

    [SerializeField] private string _taskID;

    public string TaskID => _taskID;


    public void SetTaskInfo(string taskTitle, string taskDescription, string taskID)
    {
        _taskCheck.SetActive(false);
        _taskTitle.text = taskTitle;
        _taskDescription.text = taskDescription;
        _taskID = taskID;
    }
}
