using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Interaction Settings")]
    [SerializeField] private float interactDistance = 2f;
    [SerializeField] private Transform playerCamera;
    
    [Header("UI Settings")]
    [SerializeField] private InteractionPrompt interactionPrompt;

    private void Awake() {
        if (playerCamera == null && Camera.main != null) {
            playerCamera = Camera.main.transform;
        }

        if (interactionPrompt == null) {
            interactionPrompt = FindFirstObjectByType<InteractionPrompt>(FindObjectsInactive.Include);
        }
    }

    private void Update(){
        if (playerCamera == null || interactionPrompt == null) {
            return;
        }

        checkForInteractables();
    }

    private void checkForInteractables(){
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactDistance)) {
            Interactable interactable = hit.collider.GetComponentInParent<Interactable>();
            if (interactable != null){
                interactionPrompt.showPrompt(interactable.getPrompt());

                if (Keyboard.current?.eKey.wasPressedThisFrame == true) {
                    interactionPrompt.hidePrompt();
                    interactable.Interact();
                }
            }else{
                interactionPrompt.hidePrompt();
            }
        } else {
            interactionPrompt.hidePrompt();
        }
    }

    private void OnDisable() {
        if (interactionPrompt != null) {
            interactionPrompt.hidePrompt();
        }
    }
}
