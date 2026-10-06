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
    [SerializeField] private InventoryHotbarUI hotbarUI;
    private int startFrame;
    private bool interactionWasEnabled;
    private bool hotbarWasEnabled;
    public bool CanInspect => !isInspecting && inspectPos != null &&
        playerInteraction != null && hotbarUI != null;

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
        if (hotbarUI == null)
            hotbarUI = FindFirstObjectByType<InventoryHotbarUI>(FindObjectsInactive.Include);
    }

    private void Update() {
        if (!isInspecting || currObject == null || Time.frameCount == startFrame) {
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

        if (!CanInspect || objectToInspect == null) {
            return;
        }

        isInspecting = true;
        currObject = objectToInspect;
        startFrame = Time.frameCount;
        interactionWasEnabled = playerInteraction.enabled;
        hotbarWasEnabled = hotbarUI.enabled;
        hotbarUI.enabled = false;

        initPos = currObject.transform.position;
        initRotation = currObject.transform.rotation;
        parentObject = currObject.transform.parent;

        currObject.transform.SetParent(inspectPos);
        currObject.transform.localPosition = Vector3.zero;
        currObject.transform.localRotation = Quaternion.identity;

        playerInteraction.enabled = false;

        Debug.Log("Inspecting " + currObject.name);
    }

    public bool IsCarryingPhoto() {
        GameObject item = InventoryManager.Instance?.SelectedItem;
        return item != null && item.GetComponent<EvidenceObject>() != null;
    }

    public GameObject PlacePhotoOnScanner(Transform scanPoint) {
        if (!IsCarryingPhoto() || scanPoint == null) {
            return null;
        }

        GameObject placedPhoto = InventoryManager.Instance.SelectedItem;
        if (isInspecting)
            stopInspect();
        InventoryManager.Instance.RemoveItem(placedPhoto);
        placedPhoto.SetActive(true);

        placedPhoto.transform.SetParent(scanPoint);
        placedPhoto.transform.localPosition = Vector3.zero;
        placedPhoto.transform.localRotation = Quaternion.identity;

        currObject = null;
        isInspecting = false;

        return placedPhoto;
    }


    public void stopInspect(){

        if (!isInspecting) {
            return;
        }

        isInspecting = false;

        if (InventoryManager.Instance != null)
            InventoryManager.Instance.RefreshHeldItem();
        else if (currObject != null)
        {
            currObject.transform.SetParent(parentObject);
            currObject.transform.position = initPos;
            currObject.transform.rotation = initRotation;
        }
        currObject = null;

        if (playerInteraction != null)
            playerInteraction.enabled = interactionWasEnabled;
        if (hotbarUI != null)
            hotbarUI.enabled = hotbarWasEnabled;

        Debug.Log("Stopped inspecting");

    }

    private void OnDisable()
    {
        if (isInspecting)
            stopInspect();
    }

    private void OnDestroy()
    {
        if (instance == this)
            instance = null;
    }
}
