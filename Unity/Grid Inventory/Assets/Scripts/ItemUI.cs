using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Handles dragging of items, snapping them to the grid,
// and tinting them green/red to show whether the drop spot is valid.

public class ItemUI : MonoBehaviour,
    IBeginDragHandler,
    IDragHandler,
    IEndDragHandler
{
    private Item item;
    private RectTransform rectTransform;

    [SerializeField] private Image itemImage;

    [Header("Placement Tint")]
    [SerializeField] private Color validTint = new Color(0.4f, 1f, 0.4f, 1f);
    [SerializeField] private Color invalidTint = new Color(1f, 0.4f, 0.4f, 1f);

    private Color normalColor;

    private Canvas canvas;
    private float cellSize;
    private Grid inventoryGrid;

    private Vector2 originalGridPos;
    private bool originalRotated;
    private bool isDragging;

    public void Setup(Item item, float cellSize, Grid inventoryGrid)
    {
        this.item = item;
        this.cellSize = cellSize;
        this.inventoryGrid = inventoryGrid;

        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();

        itemImage.sprite = item.img;
        normalColor = itemImage.color;

        UpdateSize(cellSize);
        UpdatePosition(cellSize);
    }

    private void Update()
    {
        if (isDragging && Input.GetKeyDown(KeyCode.R))
        {
            Rotate();
        }
    }

    public void UpdateSize(float cellSize)
    {
        rectTransform.sizeDelta = new Vector2(
            item.GetWidth() * cellSize,
            item.GetHeight() * cellSize);
    }

    public void UpdatePosition(float cellSize)
    {
        rectTransform.anchoredPosition = new Vector2(
            item.gridPos.x * cellSize,
            item.gridPos.y * cellSize);
    }

    // ---------- Placement check + tint ----------

    // The cell the item's bottom-left corner is closest to right now.
    // Based on where the item IS, so the color always matches what you see.
    private Vector2Int GetSnappedCell()
    {
        Vector2 pos = rectTransform.anchoredPosition;
        return new Vector2Int(
            Mathf.RoundToInt(pos.x / cellSize),
            Mathf.RoundToInt(pos.y / cellSize));
    }

    private void RefreshTint()
    {
        Vector2Int cell = GetSnappedCell();
        bool valid = inventoryGrid.CanPlaceItem(cell.x, cell.y, item);
        itemImage.color = valid ? validTint : invalidTint;
    }

    // ---------- Dragging ----------

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragging = true;
        originalGridPos = item.gridPos;
        originalRotated = item.rotated;

        inventoryGrid.RemoveItem(item);

        rectTransform.SetAsLastSibling(); // draw above other items while dragging
        RefreshTint();
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        RefreshTint();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;
        itemImage.color = normalColor;

        Vector2Int cell = GetSnappedCell();

        if (inventoryGrid.CanPlaceItem(cell.x, cell.y, item))
        {
            inventoryGrid.PlaceItem(cell.x, cell.y, item); // also sets item.gridPos
            UpdatePosition(cellSize);
            Debug.Log($"Placed {item.itemName} at ({cell.x}, {cell.y})");
        }
        else
        {
            ReturnToOriginalSpot();
            Debug.Log($"Invalid placement. Returning {item.itemName}");
        }
    }

    private void ReturnToOriginalSpot()
    {
        // Undo a rotation made mid-drag, or the item may no longer fit its old spot
        if (item.rotated != originalRotated)
        {
            Rotate();
        }

        item.gridPos = originalGridPos;
        inventoryGrid.PlaceItem((int)originalGridPos.x, (int)originalGridPos.y, item);
        UpdatePosition(cellSize);
    }

    private void Rotate()
    {
        item.Rotate();
        UpdateSize(cellSize);
        itemImage.transform.Rotate(0, 0, 90);

        if (isDragging)
        {
            RefreshTint(); // the footprint changed, so re-check right away
        }

        Debug.Log($"{item.itemName} rotated. Size: {item.GetWidth()}x{item.GetHeight()}");
    }

    private void OnDisable()
    {
        // If the inventory closes mid-drag, Unity never calls OnEndDrag,
        if (isDragging)
        {
            isDragging = false;
            itemImage.color = normalColor;
            ReturnToOriginalSpot();
        }
    }
}