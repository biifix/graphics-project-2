using UnityEngine;
using System;

public class SIIObject : MonoBehaviour {
    
    [Header("SII Object Setting")]
    [SerializeField] private bool visibleWhenSiiActive = true;

    private void Start() {

        SIIManager.instance.onSiiToggled += handleSiiToggle;
        handleSiiToggle(SIIManager.instance.isActive);
    }

    
    private void handleSiiToggle(bool isActive) {
        bool isVisible;

        if (visibleWhenSiiActive) {
            isVisible = isActive;
        } else {
            isVisible = !isActive;
        }

        Debug.Log(gameObject.name + " | SII Active: " + isActive + " | Visible: " + isVisible);
        gameObject.SetActive(isVisible);
    }

}
