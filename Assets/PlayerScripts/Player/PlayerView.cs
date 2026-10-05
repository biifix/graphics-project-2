using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerView : MonoBehaviour {
    [Header("View Settings")]
    [SerializeField] private float mouseSensitivity = 0.38f;
    [SerializeField] private Transform playerCamera;

    private float xRotation = 0f;

    private void Start() {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update() {
        handleMouse();
    }

    private void handleMouse() {
        Mouse mouse = Mouse.current;
        if (mouse == null) {
            return;
        }

        Vector2 mouseDelta = mouse.delta.ReadValue();
        float mouseX = mouseDelta.x * mouseSensitivity;
        float mouseY = mouseDelta.y * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }
}
