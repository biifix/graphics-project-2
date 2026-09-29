using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class TutorialPromptController : MonoBehaviour
{
    [SerializeField] private TMP_Text siiTogglePrompt;

    private int qPressCount = 0;

    public bool IsToggleTutorialComplete { get; private set; }

    private void Start()
    {
        if (siiTogglePrompt != null)
        {
            siiTogglePrompt.text = "Press [Q] to Toggle SII";
            siiTogglePrompt.gameObject.SetActive(true);
        }
    }

    private void Update()
    {
        if (siiTogglePrompt == null || !siiTogglePrompt.gameObject.activeSelf)
        {
            return;
        }

        if (Keyboard.current?.qKey.wasPressedThisFrame == true)
        {
            qPressCount++;

            if (qPressCount >= 2)
            {
                IsToggleTutorialComplete = true;
                HideMessage();
            }
        }
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
