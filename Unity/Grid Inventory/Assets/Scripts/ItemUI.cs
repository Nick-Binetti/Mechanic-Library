using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour,
    IBeginDragHandler,
    IDragHandler,
    IEndDragHandler
{
    private Item item;

    private RectTransform rectTransform;
    private Image image;

    private Canvas canvas;

    private float cellSize;

    private Grid inventoryGrid;

    private Vector2 originalGridPos;

    private bool isDragging;


    public void Setup(Item item, float cellSize, Grid inventoryGrid)
    {
        this.item = item;
        this.cellSize = cellSize;
        this.inventoryGrid = inventoryGrid;

        rectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();

        canvas = GetComponentInParent<Canvas>();

        image.sprite = item.img;

        UpdateSize(cellSize);
        UpdatePosition(cellSize);
    }

    private void Update()
    {
        //check for rotation(polling bc im lazy)
        if (isDragging && Input.GetKeyDown(KeyCode.R))
        {
            Rotate();
        }
    }

    public void UpdateSize(float cellSize)
    {
        rectTransform.sizeDelta = new Vector2(
            item.GetWidth() * cellSize,
            item.GetHeight() * cellSize
        );
    }

    public void UpdatePosition(float cellSize)
    {
        rectTransform.anchoredPosition = new Vector2(
            item.gridPos.x * cellSize,
            item.gridPos.y * cellSize
        );
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragging = true;
        originalGridPos = item.gridPos;

        inventoryGrid.RemoveItem(item);

        Debug.Log("Started dragging " + item.itemName);
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition +=
            eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
{
        isDragging = false;

        RectTransform parentRect = rectTransform.parent as RectTransform;

    Vector2 localMousePosition;

    RectTransformUtility.ScreenPointToLocalPointInRectangle(
        parentRect,
        eventData.position,
        eventData.pressEventCamera,
        out localMousePosition);

    int gridX = Mathf.FloorToInt(
        localMousePosition.x / cellSize
    );

    int gridY = Mathf.FloorToInt(
        localMousePosition.y / cellSize
    );

    // Check if the item can fit at this position
    if (inventoryGrid.CanPlaceItem(gridX, gridY, item))
    {
        inventoryGrid.PlaceItem(gridX, gridY, item);

        item.gridPos = new Vector2(gridX, gridY);

        UpdatePosition(cellSize);

        Debug.Log(
            "Placed " + item.itemName +
            " at (" + gridX + ", " + gridY + ")"
        );
    }
    else
    {
        // Return to original position
        item.gridPos = originalGridPos;

        inventoryGrid.PlaceItem((int)originalGridPos.x, (int)originalGridPos.y, item);

        UpdatePosition(cellSize);

        Debug.Log(
            "Invalid placement. Returning " +
            item.itemName
        );
    }

}
    private void Rotate()
    {
        item.Rotate();

        UpdateSize(cellSize);

        Debug.Log(
            item.itemName +
            " rotated. Size: " +
            item.GetWidth() +
            "x" +
            item.GetHeight()
        );
    }
}