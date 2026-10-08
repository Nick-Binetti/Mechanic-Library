using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private int width = 6;
    [SerializeField] private int height = 5;
    [SerializeField] private float cellSize = 80f;

    [SerializeField] private GameObject cellPrefab;

    [SerializeField] private GameObject itemUIPrefab;

    [SerializeField] private Sprite keybladeSprite;
    [SerializeField] private Sprite oblivionSprite;
    [SerializeField] private Sprite oathKeeperSprite;


    private Grid inventoryGrid;

    private void Start()
    {
        inventoryGrid = new Grid(width, height, cellSize, transform.position);

        CreateGrid();

        //eventually change out for instantiation of scriptable objects
        //Item keyblade 
        Item keyblade = ScriptableObject.CreateInstance<Item>();

        keyblade.itemName = "Keyblade";
        keyblade.width = 1;
        keyblade.height = 3;
        keyblade.img = keybladeSprite;
        keyblade.gridPos = new Vector2(1, 1);

        Item oblivion = ScriptableObject.CreateInstance<Item>();
        oblivion.itemName = "Oblivion";
        oblivion.width = 1;
        oblivion.height = 3;
        oblivion.img = oblivionSprite;
        oblivion.gridPos = new Vector2(5, 1);

        Item oathKeeper = ScriptableObject.CreateInstance<Item>();
        oathKeeper.itemName = "Oathkeeper";
        oathKeeper.width = 1;
        oathKeeper.height = 3;
        oathKeeper.img = oathKeeperSprite;
        oathKeeper.gridPos = new Vector2(9, 1);


        CreateItemUI(keyblade);
        CreateItemUI(oblivion);
        CreateItemUI(oathKeeper);
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

        //play around with OnValidate to have items build themselves outside of runtime
    }
}