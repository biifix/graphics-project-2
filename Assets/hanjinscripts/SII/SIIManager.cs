using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class SIIManager : MonoBehaviour {
    public static SIIManager instance { get; private set; }

    [Header("SII Settings")]
    [SerializeField] private bool startActive = false;
    [SerializeField] private GameObject siiHudOverlay;


    public bool isActive { get; private set; }

    // allow object to react to SII change in state
    public event Action<bool> onSiiToggled;

    private void Awake() {
        if(instance != null && instance != this) {
            Destroy(gameObject);
        } else {
            instance = this;
        }
    }

    private void Start() {
        setSiiActive(startActive);
    }

    private void Update() {
        if (Keyboard.current?.qKey.wasPressedThisFrame == true) {
            toggleSii();
        }
    }

    private void toggleSii() {
        setSiiActive(!isActive);
    }

    private void setSiiActive(bool active) {

        isActive = active;

        // Turn HUD overlay on/off
        if (siiHudOverlay != null) {
            siiHudOverlay.SetActive(isActive);
        }

        onSiiToggled?.Invoke(isActive);

        Debug.Log("SII Enabled: " + isActive);

    }
}
