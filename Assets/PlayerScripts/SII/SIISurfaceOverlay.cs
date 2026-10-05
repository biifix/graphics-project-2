using UnityEngine;

public class SIISurfaceOverlay : MonoBehaviour
{
    [SerializeField] private Renderer[] overlayRenderers;

    private void Start()
    {
        if (SIIManager.instance == null)
        {
            Debug.LogWarning(
                "SIISurfaceOverlay could not find SIIManager.",
                this
            );

            return;
        }

        SIIManager.instance.onSiiToggled += HandleSiiToggle;

        HandleSiiToggle(SIIManager.instance.isActive);
    }

    private void OnDestroy()
    {
        if (SIIManager.instance != null)
        {
            SIIManager.instance.onSiiToggled -= HandleSiiToggle;
        }
    }

    private void HandleSiiToggle(bool isActive)
    {
        foreach (Renderer renderer in overlayRenderers)
        {
            if (renderer != null)
            {
                renderer.enabled = isActive;
            }
        }
    }
}
