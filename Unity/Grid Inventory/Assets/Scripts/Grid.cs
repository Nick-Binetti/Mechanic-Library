using UnityEngine;

public class Grid
{
    private int width;
    private int height;
    private float cellSize;
    private Vector3 originPos;

    //Underlying 2D array
    private int[,] gridArray;

    //Constructor(no monobehavior makes this possible)
    public Grid(int width, int height, float cellSize, Vector3 originPos)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPos = originPos;
        gridArray = new int[width,height];

        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                Debug.Log(x + "," + y);
                Debug.Log(GetWorldPosition(x, y));
                //Create left and bottom lines for each cell
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.wheat, 100.0f);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x+1, y), Color.wheat, 100.0f);
            }
        }
        //Create lines to enclose the grid
        Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.wheat, 100.0f);
        Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.wheat, 100.0f);

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
        return new Vector3(x, y, 0) * cellSize + originPos; 
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
    public int GetValue(int x, int y)
    {
        if (x < 0 || y < 0 || x >= width || y >= height)
        {
            return -1;
        }

        return gridArray[x, y];
    }

    public void SetValue(int x, int y, int value)
    {
        if (x < 0 || y < 0 || x >= width || y >= height)
        {
            return;
        }

        gridArray[x, y] = value;
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
    public bool CanPlaceItem(int startX, int startY, int itemWidth, int itemHeight)
    {
        for (int x = 0; x < itemWidth; x++)
        {
            for (int y = 0; y < itemHeight; y++)
            {
                int gridX = startX + x;
                int gridY = startY + y;

                // Outside the grid
                if (gridX < 0 || gridY < 0 ||
                    gridX >= width || gridY >= height)
                {
                    return false;
                }

                // Cell is already occupied
                if (gridArray[gridX, gridY] != 0)
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
    public bool PlaceItem(
    int startX,
    int startY,
    int itemWidth,
    int itemHeight,
    int itemID)
    {
        if (!CanPlaceItem(startX, startY, itemWidth, itemHeight))
        {
            return false;
        }

        for (int x = 0; x < itemWidth; x++)
        {
            for (int y = 0; y < itemHeight; y++)
            {
                gridArray[startX + x, startY + y] = itemID;
            }
        }

        return true;
    }

}


