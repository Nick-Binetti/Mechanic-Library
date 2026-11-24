using UnityEngine;

// Opens and closes the inventory canvas with Tab.
public class InventoryToggle : MonoBehaviour
{
    [SerializeField] private GameObject inventoryCanvas;
    [SerializeField] private KeyCode toggleKey = KeyCode.Tab;
    [SerializeField] private bool startOpen = true;

    private void Start()
    {
        inventoryCanvas.SetActive(startOpen);
    }

    private void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            inventoryCanvas.SetActive(!inventoryCanvas.activeSelf);
        }
    }
}