using UnityEngine;

public class SIIEvidenceObject : MonoBehaviour {

    [Header("SII Version")]
    [SerializeField] private GameObject siiObject;
    [SerializeField] private GameObject physicalObject;

    private void Start() {
        
        if (SIIManager.instance == null) {
            return;
        }
        
        SIIManager.instance.onSiiToggled += handleSiiToggled;
        handleSiiToggled(SIIManager.instance.isActive);
    }

    private void OnDestroy() {
        
        if (SIIManager.instance == null) {
            return;
        }

        SIIManager.instance.onSiiToggled -= handleSiiToggled;
    }

    private void handleSiiToggled(bool isActive) {
        siiObject.SetActive(isActive);
        physicalObject.SetActive(!isActive);
    }
    
}
    