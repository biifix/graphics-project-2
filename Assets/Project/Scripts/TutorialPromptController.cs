using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class TutorialPromptController : MonoBehaviour
{
    [SerializeField] private TMP_Text siiTogglePrompt;

    private int qPressCount = 0;

    private void Start()
    {
        if (siiTogglePrompt != null)
        {
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
                siiTogglePrompt.gameObject.SetActive(false);
            }
        }
    }
}
