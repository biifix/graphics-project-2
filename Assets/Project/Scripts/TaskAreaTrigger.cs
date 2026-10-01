using UnityEngine;

public class TaskAreaTrigger : MonoBehaviour
{
    [SerializeField] private TaskManager taskManager;
    [SerializeField] private string taskToComplete;

    private bool hasTriggered;

    private void OnTriggerEnter(Collider other)
    {
        if (hasTriggered || taskManager == null)
        {
            return;
        }

        if (!other.TryGetComponent<PlayerMovement>(out _))
        {
            return;
        }

        if (taskManager.CurrentTaskId != taskToComplete)
        {
            return;
        }

        taskManager.CompleteTask(taskToComplete);
        hasTriggered = true;
    }
}