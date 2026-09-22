using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private int width = 6;
    [SerializeField] private int height = 5;
    [SerializeField] private float cellSize = 80f;

    [SerializeField] private GameObject cellPrefab;

    [SerializeField] private GameObject itemUIPrefab;

    [SerializeField] private Sprite keybladeSprite;
    [SerializeField] private Sprite oblivionSprite;

    private Grid inventoryGrid;

    private void Start()
    {
        inventoryGrid = new Grid(width, height, cellSize, transform.position);

        CreateGrid();

        Item keyblade = new Item("Keyblade", 3, 3, keybladeSprite);
        keyblade.gridPos = new Vector2(1, 1);

        Item oblivion = new Item("Oblivion", 1, 3, oblivionSprite);
        oblivion.gridPos = new Vector2(5, 1);

        CreateItemUI(keyblade);
        CreateItemUI(oblivion);
    }

    private void CreateGrid()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GameObject cell = Instantiate(cellPrefab, transform);

                RectTransform rectTransform =
                    cell.GetComponent<RectTransform>();

                rectTransform.anchoredPosition = new Vector2(x * cellSize, y * cellSize);
            }
        }
    }

    private void CreateItemUI(Item item)
    {
        inventoryGrid.PlaceItem((int)item.gridPos.x, (int)item.gridPos.y, item);

        GameObject itemObject = Instantiate(itemUIPrefab, transform);

        ItemUI itemUI = itemObject.GetComponent<ItemUI>();

        itemUI.Setup(item, cellSize, inventoryGrid);
    }
}