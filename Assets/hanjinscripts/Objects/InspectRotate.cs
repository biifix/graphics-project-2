using UnityEngine;
using UnityEngine.InputSystem;

public class InspectRotate : MonoBehaviour {

    [Header("Rotate Settings")]
    [SerializeField] private float rotationSpeed = 1f;

    public void rotate() {

        if (Mouse.current == null) {
            return;
        }

        Vector2 mouseDelta = Mouse.current.delta.ReadValue();

        float rotateX = mouseDelta.y * rotationSpeed;
        float rotateY = -mouseDelta.x * rotationSpeed;

        transform.Rotate(rotateX, rotateY, 0f, Space.World);
    }

    




}
