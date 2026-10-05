using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class StaffTaskInteractable : Interactable
{
    [Header("Task and dialogue")]
    [SerializeField] private TaskManager taskManager;
    [SerializeField] private TMP_Text dialogueText;

    [SerializeField, TextArea]
    private string[] dialogueLines =
    {
        "Welcome to the Central Hub.",
        "Your assignment is to retrieve the fire incident records.",
        "Head to Records Bay to find the files."
    };

    [SerializeField, TextArea]
    private string greeting = "Good day, mate! How's it going?";

    private bool isTaskDialogue;

    [Header("Player controls")]
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerView playerView;
    [SerializeField] private PlayerInteraction playerInteraction;

    private bool dialogueOpen;
    private int lineIndex;
    private int startFrame;

    private bool movementWasEnabled;
    private bool viewWasEnabled;
    private bool interactionWasEnabled;

    private void Awake()
    {
        if (dialogueText != null)
            dialogueText.gameObject.SetActive(false);
    }

    public override void Interact()
    {
        if (dialogueOpen)
            return;

        if (taskManager == null || dialogueText == null ||
            playerMovement == null || playerView == null ||
            playerInteraction == null)
            return;

        isTaskDialogue = taskManager.CurrentTaskId == "talk_to_staff";

        if (isTaskDialogue &&
            (dialogueLines == null || dialogueLines.Length == 0))
            return;

        movementWasEnabled = playerMovement.enabled;
        viewWasEnabled = playerView.enabled;
        interactionWasEnabled = playerInteraction.enabled;

        playerMovement.enabled = false;
        playerInteraction.enabled = false;

        dialogueOpen = true;
        lineIndex = 0;
        startFrame = Time.frameCount;

        dialogueText.gameObject.SetActive(true);
        ShowLine();
    }

    private void Update()
    {
        if (!dialogueOpen)
            return;

        if (Time.frameCount == startFrame)
            return;

        if (Keyboard.current?.eKey.wasPressedThisFrame != true)
            return;

        lineIndex++;

        if (isTaskDialogue && lineIndex < dialogueLines.Length)
        {
            ShowLine();
        }
        else
        {
            EndDialogue();

            if (isTaskDialogue)
                taskManager.CompleteTask("talk_to_staff");
        }
    }

    private void ShowLine()
    {
        string line = isTaskDialogue
            ? dialogueLines[lineIndex]
            : greeting;

        dialogueText.text = line + "\n\n[E] Continue";
    }

    private void EndDialogue()
    {
        dialogueOpen = false;

        if (dialogueText != null)
            dialogueText.gameObject.SetActive(false);

        if (playerMovement != null)
            playerMovement.enabled = movementWasEnabled;

        if (playerView != null)
            playerView.enabled = viewWasEnabled;

        if (playerInteraction != null)
            playerInteraction.enabled = interactionWasEnabled;
    }

    private void OnDisable()
    {
        if (dialogueOpen)
            EndDialogue();
    }
}
