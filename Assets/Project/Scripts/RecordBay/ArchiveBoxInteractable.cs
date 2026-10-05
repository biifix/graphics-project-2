using UnityEngine;
using UnityEngine.InputSystem;

public class ArchiveBoxInteractable : Interactable
{
    [SerializeField] private ArchiveWindowController windowController;

    [Header("Documents")]
    [SerializeField] private Sprite dutyRoster;
    [SerializeField] private Sprite photo;
    [SerializeField] private Sprite fireReport;

    public override Key InteractionKey => Key.F;

    public override void Interact()
    {
        if (windowController != null)
        {
            windowController.Open(dutyRoster, photo, fireReport);
        }
    }
}