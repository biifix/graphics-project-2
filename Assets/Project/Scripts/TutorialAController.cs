using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections;

public class TutorialAController : MonoBehaviour
{
    [Header("Archive Data")]
    [SerializeField] private string archiveId = "A-17";
    [SerializeField] private string siiDateRange = "2140-2150";
    [SerializeField] private string physicalDateRange = "2140-2150";

    [Header("Tutorial UI")]
    [SerializeField] private TutorialPromptController tutorialPrompt;

    [Tooltip("Legacy scene references. These are hidden so tutorial messages share one UI slot.")]
    [SerializeField] private TMP_Text confirmMatchText;
    [SerializeField] private TMP_Text feedbackText;

    [Header("Archive Detection")]
    [SerializeField] private Transform playerCamera;
    [SerializeField] private GameObject archiveObject;
    [SerializeField] private float detectionDistance = 3f;

    private bool confirmed = false;
    private bool lookingAtArchive = false;

    private void Start()
    {
        // Try to find the main camera automatically if not assigned
        if (playerCamera == null && Camera.main != null)
        {
            playerCamera = Camera.main.transform;
        }

        if (tutorialPrompt == null)
        {
            tutorialPrompt = FindFirstObjectByType<TutorialPromptController>();
        }

        // Keep the old scene objects for compatibility, but use the shared
        // tutorial message slot so prompts never overlap on screen.
        if (confirmMatchText != null)
        {
            confirmMatchText.gameObject.SetActive(false);
        }

        if (feedbackText != null)
        {
            feedbackText.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (confirmed)
            return;

        CheckArchiveInView();

        bool toggleTutorialComplete =
            tutorialPrompt == null || tutorialPrompt.IsToggleTutorialComplete;

        if (
            lookingAtArchive &&
            toggleTutorialComplete &&
            Keyboard.current != null &&
            Keyboard.current.cKey.wasPressedThisFrame
        )
        {
            ConfirmMatch();
        }
    }

    private void CheckArchiveInView()
    {
        lookingAtArchive = false;

        if (playerCamera == null || archiveObject == null)
        {
            if (confirmMatchText != null)
            {
                confirmMatchText.gameObject.SetActive(false);
            }

            return;
        }

        Ray ray = new Ray(
            playerCamera.position,
            playerCamera.forward
        );

        if (Physics.Raycast(ray, out RaycastHit hit, detectionDistance))
        {
            Transform hitTransform = hit.collider.transform;

            // Accept either A-17 itself or any child object of A-17
            if (
                hitTransform == archiveObject.transform ||
                hitTransform.IsChildOf(archiveObject.transform)
            )
            {
                lookingAtArchive = true;
            }
        }

        if (tutorialPrompt != null && tutorialPrompt.IsToggleTutorialComplete)
        {
            if (lookingAtArchive)
            {
                tutorialPrompt.ShowMessage("Press [C] to Confirm Match");
            }
            else
            {
                tutorialPrompt.HideMessage();
            }
        }
    }

    private IEnumerator HideFeedbackAfterDelay()
    {
        yield return new WaitForSeconds(3f);

        if (tutorialPrompt != null)
        {
            tutorialPrompt.HideMessage();
        }
        else if (feedbackText != null)
        {
            feedbackText.gameObject.SetActive(false);
        }
    }

    public void ConfirmMatch()
    {
        bool idMatches = archiveId == "A-17";
        bool dateMatches = siiDateRange == physicalDateRange;

        if (idMatches && dateMatches)
        {
            confirmed = true;

            if (tutorialPrompt != null)
            {
                tutorialPrompt.ShowMessage("Match confirmed.");
            }
            else if (feedbackText != null)
            {
                feedbackText.text = "Match confirmed.";
                feedbackText.gameObject.SetActive(true);
            }

            StartCoroutine(HideFeedbackAfterDelay());

            Debug.Log("Tutorial A: Match confirmed.");
        }
        else
        {
            if (tutorialPrompt != null)
            {
                tutorialPrompt.ShowMessage("Records do not match.");
            }
            else if (feedbackText != null)
            {
                feedbackText.text = "Records do not match.";
                feedbackText.gameObject.SetActive(true);
            }

            Debug.Log("Tutorial A: Records do not match.");
        }
    }
}
