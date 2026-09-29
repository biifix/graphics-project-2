using UnityEngine;

public class IntroTutorialController : MonoBehaviour
{
    [Header("Intro UI")]
    [SerializeField] private GameObject introTutorialPanel;

    [Header("Player Controls")]
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerView playerView;

    [Header("Tutorial Prompt")]
    [SerializeField] private TutorialPromptController tutorialPrompt;

    [SerializeField] private GameObject taskTracker;

    private void Start()
    {
        ShowIntro();
    }

    private void ShowIntro()
    {
        introTutorialPanel.SetActive(true);
        taskTracker.SetActive(false);

        if (playerMovement != null)
        {
            playerMovement.enabled = false;
        }

        if (playerView != null)
        {
            playerView.enabled = false;
        }

        if (tutorialPrompt != null)
        {
            tutorialPrompt.HideMessage();
            tutorialPrompt.enabled = false;
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void BeginShift()
    {
        introTutorialPanel.SetActive(false);
        taskTracker.SetActive(true);

        if (playerMovement != null)
        {
            playerMovement.enabled = true;
        }

        if (playerView != null)
        {
            playerView.enabled = true;
        }

        if (tutorialPrompt != null)
        {
            tutorialPrompt.enabled = true;
            tutorialPrompt.ShowMessage(
                "Press [Q] to Toggle SII"
            );
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
} 
