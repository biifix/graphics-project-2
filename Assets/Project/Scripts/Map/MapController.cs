using UnityEngine;
using UnityEngine.InputSystem;

public class MapController : MonoBehaviour
{
    [Header("Map UI")]
    [SerializeField] private GameObject mapPanel;

    [Header("Player Marker")]
    [SerializeField] private RectTransform playerMarker;
    [SerializeField] private RectTransform[] areaMarkerPoints;
    [SerializeField] private int startingArea = 0;

    [Header("Player Controls")]
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerView playerView;

    [Header("Other UI")]
    [SerializeField] private GameObject taskTracker;

    private bool isMapOpen;
    private int currentArea;

    private void Start()
    {
        isMapOpen = false;
        mapPanel.SetActive(false);

        currentArea = startingArea;
        UpdatePlayerMarker();
    }

    private void Update()
    {
        if (Keyboard.current?.mKey.wasPressedThisFrame == true)
        {
            ToggleMap();
        }
    }

    public void ToggleMap()
    {
        if (isMapOpen)
        {
            CloseMap();
        }
        else
        {
            OpenMap();
        }
    }

    private void OpenMap()
    {
        isMapOpen = true;
        mapPanel.SetActive(true);

        UpdatePlayerMarker();

        if (playerMovement != null)
        {
            playerMovement.enabled = false;
        }

        if (playerView != null)
        {
            playerView.enabled = false;
        }

        if (taskTracker != null)
        {
            taskTracker.SetActive(false);
        }
    }

    private void CloseMap()
    {
        isMapOpen = false;
        mapPanel.SetActive(false);

        if (playerMovement != null)
        {
            playerMovement.enabled = true;
        }

        if (playerView != null)
        {
            playerView.enabled = true;
        }

        if (taskTracker != null)
        {
            taskTracker.SetActive(true);
        }
    }

    public void SetCurrentArea(int areaIndex)
    {
        if (
            areaIndex < 0 ||
            areaIndex >= areaMarkerPoints.Length
        )
        {
            Debug.LogWarning(
                "Invalid map area index: " + areaIndex
            );

            return;
        }

        currentArea = areaIndex;
        UpdatePlayerMarker();
    }

    private void UpdatePlayerMarker()
    {
        if (
            playerMarker == null ||
            areaMarkerPoints == null ||
            areaMarkerPoints.Length == 0
        )
        {
            return;
        }

        RectTransform targetPoint =
            areaMarkerPoints[currentArea];

        if (targetPoint == null)
        {
            return;
        }

        playerMarker.position = targetPoint.position;
    }
}