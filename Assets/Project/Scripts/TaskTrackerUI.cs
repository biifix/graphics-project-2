using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class TaskTrackerUI : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject contentPanel;

    [Header("Text")]
    [SerializeField] private TMP_Text taskTitleText;
    [SerializeField] private TMP_Text objectiveText;
    [SerializeField] private TMP_Text toggleIconText;

    [Header("Starting State")]
    [SerializeField] private bool startExpanded = false;

    private bool isExpanded;

    private void Start()
    {
        SetExpanded(startExpanded);
    }

    private void Update()
    {
        if (Keyboard.current?.tabKey.wasPressedThisFrame == true)
        {
            Toggle();
        }
    }

    public void Toggle()
    {
        SetExpanded(!isExpanded);
    }

    private void SetExpanded(bool expanded)
    {
        isExpanded = expanded;

        if (contentPanel != null)
        {
            contentPanel.SetActive(isExpanded);
        }

        if (toggleIconText != null)
        {
            toggleIconText.text = isExpanded ? "▲" : "▼";
        }
    }

    public void SetTask(string title, string objective)
    {
        if (taskTitleText != null)
        {
            taskTitleText.text = title;
        }

        if (objectiveText != null)
        {
            objectiveText.text = objective;
        }
    }
}
