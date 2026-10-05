using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ArchiveWindowController : MonoBehaviour
{
    [Header("Window")]
    [SerializeField] private GameObject archiveWindow;
    [SerializeField] private Image dutyRosterImage;
    [SerializeField] private Image photoImage;
    [SerializeField] private Image fireReportImage;

    [Header("Player")]
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerView playerView;
    [SerializeField] private PlayerInteraction playerInteraction;

    public bool IsOpen { get; private set; }

    private bool movementWasEnabled;
    private bool viewWasEnabled;
    private bool interactionWasEnabled;
    private CursorLockMode previousCursorLock;
    private bool previousCursorVisible;

    private void Awake()
    {
        if (archiveWindow != null)
            archiveWindow.SetActive(false);
    }

    private void Update()
    {
        if (IsOpen &&
            Keyboard.current?.escapeKey.wasPressedThisFrame == true)
        {
            Close();
        }
    }

    public void Open(Sprite roster, Sprite photo, Sprite report)
    {
        if (IsOpen)
            return;

        if (archiveWindow == null || dutyRosterImage == null ||
            photoImage == null || fireReportImage == null ||
            playerMovement == null || playerView == null ||
            playerInteraction == null)
        {
            Debug.LogWarning("Archive window references are missing.", this);
            return;
        }

        SetImage(dutyRosterImage, roster);
        SetImage(photoImage, photo);
        SetImage(fireReportImage, report);

        movementWasEnabled = playerMovement.enabled;
        viewWasEnabled = playerView.enabled;
        interactionWasEnabled = playerInteraction.enabled;
        previousCursorLock = Cursor.lockState;
        previousCursorVisible = Cursor.visible;

        IsOpen = true;

        playerMovement.enabled = false;
        playerView.enabled = false;
        playerInteraction.enabled = false;

        archiveWindow.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Close()
    {
        if (!IsOpen)
            return;

        IsOpen = false;

        if (archiveWindow != null)
            archiveWindow.SetActive(false);

        if (playerMovement != null)
            playerMovement.enabled = movementWasEnabled;

        if (playerView != null)
            playerView.enabled = viewWasEnabled;

        if (playerInteraction != null)
            playerInteraction.enabled = interactionWasEnabled;

        Cursor.lockState = previousCursorLock;
        Cursor.visible = previousCursorVisible;
    }

    private void SetImage(Image image, Sprite sprite)
    {
        image.sprite = sprite;
        image.preserveAspect = true;
        image.gameObject.SetActive(sprite != null);
    }

    private void OnDisable()
    {
        Close();
    }
}
