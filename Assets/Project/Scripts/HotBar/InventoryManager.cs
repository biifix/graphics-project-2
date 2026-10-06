using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    [SerializeField] private InventoryHotbarUI hotbarUI;
    [SerializeField] private Transform holdPoint;

    private readonly GameObject[] items = new GameObject[3];
    private int displayedIndex = -1;

    public GameObject SelectedItem
    {
        get
        {
            if (hotbarUI == null)
                return null;

            return items[hotbarUI.SelectedIndex];
        }
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;
    }

    private void Update()
    {
        if (hotbarUI == null || holdPoint == null)
            return;

        if (displayedIndex != hotbarUI.SelectedIndex)
            RefreshHeldItem();
    }

    public bool TryAddItem(GameObject item)
    {
        if (item == null || hotbarUI == null || holdPoint == null)
            return false;
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == item)
            {
                hotbarUI.SelectSlot(i);
                RefreshHeldItem();
                return true;
            }
        }
        int index = hotbarUI.SelectedIndex;

        if (items[index] != null)
        {
            index = System.Array.FindIndex(
                items, storedItem => storedItem == null
            );
        }

        if (index < 0)
        {
            Debug.Log("Inventory full");
            return false;
        }

        items[index] = item;
        hotbarUI.SelectSlot(index);
        foreach (Collider itemCollider in item.GetComponentsInChildren<Collider>(true))
            itemCollider.enabled = false;
        foreach (Rigidbody body in item.GetComponentsInChildren<Rigidbody>(true))
        {
            body.isKinematic = true;
            body.useGravity = false;
        }
        RefreshHeldItem();
        return true;
    }

    public void RemoveItem(GameObject item)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == item)
                items[i] = null;
        }
        RefreshHeldItem();
    }

    public void RefreshHeldItem()
    {
        if (hotbarUI == null || holdPoint == null)
            return;

        displayedIndex = hotbarUI.SelectedIndex;

        for (int i = 0; i < items.Length; i++)
        {
            GameObject item = items[i];

            if (item == null)
                continue;

            item.transform.SetParent(holdPoint, true);
            item.transform.localPosition = Vector3.zero;
            item.transform.localRotation = Quaternion.identity;
            item.SetActive(i == displayedIndex);
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
}
