using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Objects/Item")]
public class Item : ScriptableObject
{
    public string itemName;
    public Sprite img;

    //reference to grid
    [SerializeField]
    Grid gridInventory;

    [Header("Grid Size")]
    public int width = 1;
    public int height = 1;

    public Vector2 gridPos;

    private bool rotated;

    //Constructor
    public Item(string name, int width, int height, Sprite img = null)
    {
        this.itemName = name;
        this.img = img;
        this.width = width;
        this.height = height;
    }

    //Methods
    public int GetWidth()
    {
        if(rotated)
        {
            return height;
        }
        return width;
    }
    public int GetHeight()
    {
        if (rotated)
        {
            return width;
        }
        return height;
    }

    public void Rotate()
    {
        rotated = !rotated;
    }
}
