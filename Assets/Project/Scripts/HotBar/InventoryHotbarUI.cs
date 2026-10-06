using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InventoryHotbarUI : MonoBehaviour
{
    [SerializeField] private Image[] slotBorders = new Image[3];

    [SerializeField] private Color normalColor = Color.gray;
    [SerializeField] private Color selectedColor = Color.black;

    public int SelectedIndex { get; private set; }

    private void Start()
    {
        RefreshSelection();
    }

    private void Update()
    {
        if (Mouse.current == null)
            return;

        float scroll = Mouse.current.scroll.ReadValue().y;

        if (scroll > 0f)
            SelectedIndex = (SelectedIndex + 2) % 3;
        else if (scroll < 0f)
            SelectedIndex = (SelectedIndex + 1) % 3;
        else
            return;

        RefreshSelection();
    }

    public void SelectSlot(int index)
    {
        SelectedIndex = Mathf.Clamp(index, 0, 2);
        RefreshSelection();
    }

    private void RefreshSelection()
    {
        for (int i = 0; i < 3; i++)
        {
            if (slotBorders[i] != null)
            {
                slotBorders[i].color = i == SelectedIndex
                    ? selectedColor
                    : normalColor;
            }
        }
    }
}
