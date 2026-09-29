using UnityEngine;

public class MapAreaTrigger : MonoBehaviour
{
    [SerializeField] private MapController mapController;

    [Tooltip(
        "0 Tutorial, 1 Central Hub, 2 Records Bay, " +
        "3 Photo Station, 4 Provenance Terminal, " +
        "5 Reconciliation Chamber"
    )]
    [SerializeField] private int areaIndex;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<PlayerMovement>(out _))
        {
            return;
        }

        if (mapController != null)
        {
            mapController.SetCurrentArea(areaIndex);
        }
    }
}