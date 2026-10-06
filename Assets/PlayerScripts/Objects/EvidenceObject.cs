using UnityEngine;

public class EvidenceObject : Interactable {

    [Header("Evidence Settings")]
    [SerializeField] private string evidenceName = "Item";

    public override void Interact() {
        if (InventoryManager.Instance == null ||
            InteractionManager.instance == null ||
            !InteractionManager.instance.CanInspect)
            return;

        if (!InventoryManager.Instance.TryAddItem(gameObject))
            return;

        Debug.Log("Interacted with " + evidenceName);
        InteractionManager.instance.startInspect(gameObject);
    }
}
