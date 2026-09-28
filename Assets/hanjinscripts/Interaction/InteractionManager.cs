using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionManager : MonoBehaviour {

    public static InteractionManager instance { 
        get; 
        private set; 
    }

    [Header("Inspect Settings")]
    [SerializeField] private Transform inspectPos;

    [Header("Player")]
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerView playerView;
    [SerializeField] private PlayerInteraction playerInteraction;
    
    private GameObject currObject;

    private Vector3 initPos;
    private Quaternion initRotation;
    private Transform parentObject;

    private bool isInspecting {
        get;
        set;
    }

    private void Awake() {
        if (instance != null && instance != this) {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    private void Update() {
        if (!isInspecting) {
            return;
        }

        InspectRotate rotateObject = currObject.GetComponent<InspectRotate>();

        if (rotateObject != null) {
            rotateObject.rotate();
        }    

        if (Keyboard.current?.eKey.wasPressedThisFrame == true || Keyboard.current?.escapeKey.wasPressedThisFrame == true) {
            stopInspect();
        }
    }

    public void startInspect(GameObject objectToInspect) {

        if (isInspecting) {
            return;
        }

        isInspecting = true;
        currObject = objectToInspect;

        initPos = currObject.transform.position;
        initRotation = currObject.transform.rotation;
        parentObject = currObject.transform.parent;

        currObject.transform.SetParent(inspectPos);
        currObject.transform.localPosition = Vector3.zero;
        currObject.transform.localRotation = Quaternion.identity;

        // Disable player movement
        /*
        playerMovement.enabled = false;
        playerView.enabled = false;
        */
        playerInteraction.enabled = false;

        Debug.Log("Inspecting " + currObject.name);
    }

    public bool IsCarryingPhoto() {
        return isInspecting && currObject != null;
    }

    public GameObject PlacePhotoOnScanner(Transform scanPoint) {
        if (!IsCarryingPhoto() || scanPoint == null) {
            return null;
        }

        GameObject placedPhoto = currObject;

        placedPhoto.transform.SetParent(scanPoint);
        placedPhoto.transform.localPosition = Vector3.zero;
        placedPhoto.transform.localRotation = Quaternion.identity;

        currObject = null;
        isInspecting = false;
        playerInteraction.enabled = true;

        return placedPhoto;
    }

        
    public void stopInspect(){

        if (!isInspecting) {
            return;
        }

        isInspecting = false;

        currObject.transform.SetParent(parentObject);
        currObject.transform.position = initPos;
        currObject.transform.rotation = initRotation;
        currObject = null;

        // enable player movement
        /*
        playerMovement.enabled = true;
        playerView.enabled = true;
        */
        playerInteraction.enabled = true;

        Debug.Log("Stopped inspecting");
    
    }
}
