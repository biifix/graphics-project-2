using UnityEngine;
using UnityEngine.InputSystem;

/*
handles WASD movement + gravity
    - first person
    - keep POV logic seperate to another file

*/

[RequireComponent(typeof(CharacterController))]

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("Gravity Settings")]
    [SerializeField] private float gravity = -9.81f;
    
    private CharacterController controller;
    private Vector3 velocity;

    private void Awake() {
        controller = GetComponent<CharacterController>();
    }

    private void Update() {
        handleMovement();
        handleGravity();
    }

    private void handleMovement() {
        Keyboard keyboard = Keyboard.current;
        if (keyboard == null) {
            return;
        }

        float x = 0f;
        float z = 0f;

        if (keyboard.dKey.isPressed) x += 1f;
        if (keyboard.aKey.isPressed) x -= 1f;
        if (keyboard.wKey.isPressed) z += 1f;
        if (keyboard.sKey.isPressed) z -= 1f;

        // Move in the facing direction.
        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * moveSpeed * Time.deltaTime);
    }

    private void handleGravity() {
        if (controller.isGrounded && velocity.y < 0) {
            velocity.y = -2f;
        }
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
