using UnityEngine;

public class EvidenceObject : Interactable {

    [Header("Evidence Settings")]
    [SerializeField] private string evidenceName = "Item";

    public override void Interact() {
        Debug.Log("Interacted with " + evidenceName);
        InteractionManager.instance.startInspect(gameObject);
    }
}
