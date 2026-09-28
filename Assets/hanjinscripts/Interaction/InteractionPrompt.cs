using UnityEngine;
using TMPro;

public class InteractionPrompt : MonoBehaviour {

    [Header("Prompt Settings")]
    [SerializeField] private TMP_Text promptText;

    private void Awake() {
        if (promptText == null) {
            promptText = GetComponentInChildren<TMP_Text>(true);
        }

        if (promptText == null) {
            Debug.LogError("InteractionPrompt needs a TMP_Text component assigned or placed on a child object.", this);
        }
    }

    private void Start() {
        hidePrompt();
    }
    

    public void hidePrompt() {
        if (promptText == null) {
            return;
        }

        promptText.gameObject.SetActive(false);
    }

    public void showPrompt(string prompt) {
        if (promptText == null) {
            return;
        }

        promptText.text = prompt;
        promptText.gameObject.SetActive(true);
    }

}
