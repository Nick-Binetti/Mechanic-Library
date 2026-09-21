using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Objects/Item")]
public class Item : ScriptableObject
{
    public Sprite img;

    [Header("Grid Size")]
    public int width = 1;
    public int height = 1;

    //Constructor
    public Item(Sprite img, int width, int height)
    {
        this.img = img;
        this.width = width;
        this.height = height;
    }
}
