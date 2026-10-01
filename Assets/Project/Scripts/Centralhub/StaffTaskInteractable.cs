using UnityEngine;

public class StaffTaskInteractable : Interactable
{
    [SerializeField] private TaskManager taskManager;

    public override void Interact()
    {
        base.Interact();

        if (taskManager == null)
        {
            return;
        }

        if (taskManager.CurrentTaskId != "talk_to_staff")
        {
            return;
        }

        Debug.Log("Staff conversation completed.");

        taskManager.CompleteTask("talk_to_staff");
    }
}