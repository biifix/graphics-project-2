using UnityEngine;

public class TutorialDeskTaskTrigger : MonoBehaviour
{
    [SerializeField] private TaskManager taskManager;

    private bool hasTriggered;

    private void OnTriggerEnter(Collider other)
    {
        if (hasTriggered)
        {
            return;
        }

        if (!other.TryGetComponent<PlayerMovement>(out _))
        {
            return;
        }

        if (taskManager == null)
        {
            return;
        }

        hasTriggered = true;
        taskManager.CompleteTask("reach_tutorial_desk");
    }
}