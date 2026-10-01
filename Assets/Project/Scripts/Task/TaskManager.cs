using System;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    [Serializable]
    public class TaskData
    {
        public string id;

        [TextArea]
        public string objective;
    }

    [Header("UI")]
    [SerializeField] private TaskTrackerUI taskTrackerUI;

    [Header("Tasks")]
    [SerializeField] private List<TaskData> tasks = new List<TaskData>();

    private int currentTaskIndex = -1;

    public string CurrentTaskId
    {
        get
        {
            if (currentTaskIndex < 0 || currentTaskIndex >= tasks.Count)
            {
                return "";
            }

            return tasks[currentTaskIndex].id;
        }
    }

    public void StartTasks()
    {
        if (tasks.Count == 0)
        {
            return;
        }

        currentTaskIndex = 0;
        ShowCurrentTask();
    }

    public void CompleteTask(string expectedTaskId)
    {
        if (CurrentTaskId != expectedTaskId)
        {
            return;
        }

        currentTaskIndex++;

        if (currentTaskIndex < tasks.Count)
        {
            ShowCurrentTask();
        }
    }

    private void ShowCurrentTask()
    {
        TaskData currentTask = tasks[currentTaskIndex];

        taskTrackerUI.SetTask(currentTask.objective);
    }
}
