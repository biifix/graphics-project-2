using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SourceRelationshipController : MonoBehaviour
{
    [System.Serializable]
    private class SourceNode
    {
        public string sourceName;
        public Button button;
        public Image connectionLine;
        public bool shouldConnect;

        [HideInInspector]
        public bool isSelected;
    }

    [SerializeField] private SourceNode[] sourceNodes;
    [SerializeField] private Button transcriptionButton;
    [SerializeField] private TMP_Text statusText;
    [SerializeField] private DissolveController dissolveController;

    [SerializeField] private Color normalColor = Color.white;

    [SerializeField] private Color selectedColor =
        new Color(0.2f, 0.85f, 0.75f, 1f);

    [SerializeField] private Color unselectedLineColor =
        new Color(0.35f, 0.35f, 0.35f, 1f);

    private bool completed;

    private void Start()
    {
        for (int i = 0; i < sourceNodes.Length; i++)
        {
            int index = i;

            sourceNodes[i].isSelected = false;
            sourceNodes[i].button.image.color = normalColor;
            sourceNodes[i].connectionLine.color = unselectedLineColor;

            sourceNodes[i].button.onClick.AddListener(
                () => ToggleSource(index)
            );
        }

        transcriptionButton.onClick.AddListener(ValidateSources);
    }

    private void ToggleSource(int index)
    {
        if (completed)
        {
            return;
        }

        SourceNode node = sourceNodes[index];
        node.isSelected = !node.isSelected;

        node.button.image.color =
            node.isSelected ? selectedColor : normalColor;

        node.connectionLine.color =
            node.isSelected ? selectedColor : unselectedLineColor;

        statusText.text = node.isSelected
            ? node.sourceName + " connected."
            : node.sourceName + " disconnected.";
    }

    private void ValidateSources()
    {
        if (completed)
        {
            return;
        }

        foreach (SourceNode node in sourceNodes)
        {
            if (node.isSelected != node.shouldConnect)
            {
                statusText.text =
                    "Some source connections are incorrect. Try again.";

                return;
            }
        }

        completed = true;

        statusText.text =
            "Connection confirmed: most sources originate from the 2189 transcription. Record processed.";

        foreach (SourceNode node in sourceNodes)
        {
            node.button.interactable = false;
        }

        transcriptionButton.interactable = false;
        dissolveController.BeginDissolve();
    }
}