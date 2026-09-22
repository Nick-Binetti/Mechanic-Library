using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using static UnityEditor.Progress;

public class Grid
{
    private int width;
    private int height;
    private float cellSize;
    private Vector2 originPos;

    //Underlying 2D array
    private Item[,] gridArray;

    //Constructor(no monobehavior makes this possible)
    public Grid(int width, int height, float cellSize, Vector3 originPos)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPos = originPos;
        gridArray = new Item[width,height];

        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                Debug.Log(x + "," + y);
                //Debug.Log(GetWorldPosition(x, y));
                ////Create left and bottom lines for each cell
                //Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.wheat, 100.0f);
                //Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x+1, y), Color.wheat, 100.0f);
            }
        }
        ////Create lines to enclose the grid
        //Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.wheat, 100.0f);
        //Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.wheat, 100.0f);

    }

    //Methods

    /// <summary>
    /// Converts grids coodinates into a world position
    /// </summary>
    /// <param name="x"> grid x coordinate </param>
    /// <param name="y"> grid y coordinate </param>
    /// <returns></returns>
    private Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector2(x, y) * cellSize + originPos; 
    }

    /// <summary>
    /// Converts a world position into grid coordinates
    /// </summary>
    /// <param name="worldPos"> world space vector </param>
    /// <param name="x"> grid x coord to be found based on worldPos </param>
    /// <param name="y"> grid x coord to be found based on worldPos </param>
    private void GetXY(Vector3 worldPos, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPos.x - originPos.x) /cellSize);
        y = Mathf.FloorToInt((worldPos.y -originPos.y)/ cellSize);
    }
    /// <summary>
    /// Gets item stored in single cell
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public Item GetItem(int x, int y)
    {
        if (x < 0 || y < 0 || x >= width || y >= height)
        {
            return null;
        }

        return gridArray[x, y];
    }

    /// <summary>
    /// Sets item to store in single cell
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="item"></param>
    public void SetItem(int x, int y, Item item)
    {
        if (x < 0 || y < 0 || x >= width || y >= height)
        {
            return;
        }

        gridArray[x, y] = item;
    }
    /// <summary>
    /// Checks the grid to see if the item is being placed
    /// in a grid space or if the space its trying to occupy
    /// is already filled
    /// </summary>
    /// <param name="startX"> X coord origin of item </param>
    /// <param name="startY"> Y coord origin of item </param>
    /// <param name="itemWidth"> items width </param>
    /// <param name="itemHeight"> items height </param>
    /// <returns></returns>
    public bool CanPlaceItem(int startX, int startY, Item item)
    {
        for (int x = 0; x < item.GetWidth(); x++)
        {
            for (int y = 0; y < item.GetHeight(); y++)
            {
                int gridX = startX + x;
                int gridY = startY + y;

                // Outside the grid
                if (gridX < 0 || gridY < 0 ||
                    gridX >= width || gridY >= height)
                {
                    return false;
                }

                // Cell is already occupied by another item(not itself) 
                else if (gridArray[gridX, gridY] != null && gridArray[gridX, gridY] != item)
                {
                    return false;
                }
            }
        }

        return true;
    }
    /// <summary>
    /// Actually places the item based on the check
    /// </summary>
    /// <param name="startX"> X coord origin of item </param>
    /// <param name="startY"> Y coord origin of item </param>
    /// <param name="itemWidth"> items width </param>
    /// <param name="itemHeight"> items height </param>
    /// <param name="itemID"></param>
    /// <returns></returns>
    public Vector2 PlaceItem(int startX, int startY, Item item)
    {
        //Vector2 itemPos = Vector2.zero;

        if (!CanPlaceItem(startX, startY, item))
        {
            return new Vector2(-1,-1);
        }

        else
        {
            for (int x = 0; x < item.GetWidth(); x++)
            {
                for (int y = 0; y < item.GetHeight(); y++)
                {
                   //place item
                   gridArray[startX + x, startY + y] = item;
                    //set item grid position to bottom left cell
                   item.gridPos = new Vector2(startX, startY);
                }
            }
            return item.gridPos;
        }
    }
    /// <summary>
    /// Checks for item to remove and removes it instances from the necessary cells
    /// </summary>
    /// <param name="item"></param>
    public void RemoveItem(Item item)
    {
        int startX = (int)item.gridPos.x;
        int startY = (int)item.gridPos.y;

        for (int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                if (gridArray[x, y] == item)
                {
                    //reset occupied cells
                    gridArray[x, y] = null;
                }
            }
        }
    }

    /// <summary>
    /// Removes item from current position and places it in new one(after checking ofc)
    /// </summary>
    /// <param name="newX"></param>
    /// <param name="newY"></param>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool MoveItem(int newX, int newY, Item item)
    {
        if(!CanPlaceItem(newX, newY, item))
        {
            return false;
        }
        else
        {
            RemoveItem(item);

            PlaceItem(newX, newY, item);
            return true;
        }
    }

    /// <summary>
    /// Rotates item and checks if it still fits within grid
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool RotateItem(Item item)
    {

        item.Rotate();

        //check if item still fits post rotation
        if(CanPlaceItem((int)item.gridPos.x,(int)item.gridPos.y,item))
        {
            RemoveItem(item);
            PlaceItem((int)item.gridPos.x, (int)item.gridPos.y, item);

            return true;
        }
        else
        {
            //undo rotation
            item.Rotate();
            return false;
        }
    }
}


