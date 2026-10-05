using UnityEngine;

public class CabinetDoorInteractable : Interactable
{
    [SerializeField] private float openAngle = 90f;
    [SerializeField] private float rotationSpeed = 180f;

    private Quaternion closedRotation;
    private Quaternion openRotation;
    private bool isOpen;

    private void Awake()
    {
        closedRotation = transform.localRotation;
        openRotation = closedRotation *
            Quaternion.Euler(0f, openAngle, 0f);
    }

    public override void Interact()
    {
        isOpen = !isOpen;
    }

    private void Update()
    {
        Quaternion target = isOpen
            ? openRotation
            : closedRotation;

        transform.localRotation = Quaternion.RotateTowards(
            transform.localRotation,
            target,
            rotationSpeed * Time.deltaTime
        );
    }
}