using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class TutorialPromptController : MonoBehaviour
{
    [SerializeField] private TMP_Text siiTogglePrompt;
    [SerializeField] private TaskManager taskManager;

    public bool IsToggleTutorialComplete { get; private set; }

    private void Start()
    {
        HideMessage();
    }

    private void Update()
    {
        if (Keyboard.current?.qKey.wasPressedThisFrame != true)
        {
            return;
        }

        if (taskManager == null)
        {
            return;
        }

        if (taskManager.CurrentTaskId != "turn_off_sii")
        {
            return;
        }

        IsToggleTutorialComplete = true;
        taskManager.CompleteTask("turn_off_sii");
        HideMessage();
    }

    public void ShowMessage(string message)
    {
        if (siiTogglePrompt == null)
        {
            return;
        }

        siiTogglePrompt.text = message;
        siiTogglePrompt.gameObject.SetActive(true);
    }

    public void HideMessage()
    {
        if (siiTogglePrompt == null)
        {
            return;
        }

        siiTogglePrompt.gameObject.SetActive(false);
    }
}
