using UnityEngine;
using UnityEngine.InputSystem;

public class PhotoScannerInteraction : MonoBehaviour
{
    [Header("Scanner")]
    [SerializeField] private Transform scanPoint;
    [SerializeField] private Transform playerCamera;
    [SerializeField] private float interactDistance = 2f;

    [Header("Prompt")]
    [SerializeField] private InteractionPrompt interactionPrompt;
    [SerializeField] private string scannerPrompt = "Press [F] to scan photo";

    [Header("Player")]
    [SerializeField] private GameObject playerCameraObject;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerView playerView;
    [SerializeField] private PlayerInteraction playerInteraction;

    [Header("Photo Inspection")]
    [SerializeField] private GameObject inspectionCamera;
    [SerializeField] private GameObject inspectionUI;
    [SerializeField] private GameObject inspectionDisplay;

    private bool isScanning;
    private bool scannerPromptVisible;

    private void Awake()
    {
        if (interactionPrompt == null)
        {
            interactionPrompt = FindFirstObjectByType<InteractionPrompt>(FindObjectsInactive.Include);
        }
    }

    private void Update()
    {
        if (isScanning)
        {
            HideScannerPrompt();

            if (Keyboard.current?.escapeKey.wasPressedThisFrame == true)
            {
                ExitScanner();
            }

            return;
        }

        bool canUseScanner = IsLookingAtScannerWithPhoto();

        if (canUseScanner)
        {
            interactionPrompt?.showPrompt(scannerPrompt);
            scannerPromptVisible = true;
        }
        else
        {
            HideScannerPrompt();
        }

        if (canUseScanner && Keyboard.current?.fKey.wasPressedThisFrame == true)
        {
            TryStartScanner();
        }
    }

    private bool IsLookingAtScannerWithPhoto()
    {
        if (InteractionManager.instance == null ||
            !InteractionManager.instance.IsCarryingPhoto() ||
            playerCamera == null)
        {
            return false;
        }

        Ray ray = new Ray(playerCamera.position, playerCamera.forward);

        if (!Physics.Raycast(ray, out RaycastHit hit, interactDistance))
        {
            return false;
        }

        PhotoScannerInteraction scanner =
            hit.collider.GetComponentInParent<PhotoScannerInteraction>();

        return scanner == this;
    }

    private void TryStartScanner()
    {
        if (InteractionManager.instance == null ||
            !InteractionManager.instance.IsCarryingPhoto())
        {
            return;
        }

        if (playerCamera == null)
        {
            return;
        }

        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        RaycastHit hit;

        if (!Physics.Raycast(ray, out hit, interactDistance))
        {
            return;
        }

        PhotoScannerInteraction scanner =
            hit.collider.GetComponentInParent<PhotoScannerInteraction>();

        if (scanner != this)
        {
            return;
        }

        GameObject placedPhoto =
            InteractionManager.instance.PlacePhotoOnScanner(scanPoint);

        if (placedPhoto == null)
        {
            return;
        }

        EnterScanner();
    }

    private void EnterScanner()
    {
        isScanning = true;
        HideScannerPrompt();

        playerMovement.enabled = false;
        playerView.enabled = false;
        playerInteraction.enabled = false;

        playerCameraObject.SetActive(false);
        inspectionCamera.SetActive(true);
        inspectionUI.SetActive(true);
        inspectionDisplay.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void ExitScanner()
    {
        isScanning = false;

        inspectionCamera.SetActive(false);
        inspectionUI.SetActive(false);
        inspectionDisplay.SetActive(false);
        playerCameraObject.SetActive(true);

        playerMovement.enabled = true;
        playerView.enabled = true;
        playerInteraction.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnDisable()
    {
        HideScannerPrompt();
    }

    private void HideScannerPrompt()
    {
        if (scannerPromptVisible && interactionPrompt != null)
        {
            interactionPrompt.hidePrompt();
        }

        scannerPromptVisible = false;
    }
}
