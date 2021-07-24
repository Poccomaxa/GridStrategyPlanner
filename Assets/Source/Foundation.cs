using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Foundation : MonoBehaviour
{
    private GridCell[,] foundationGrid;

    [SerializeField]
    private GridCell[] serializedGrid;
    [SerializeField]
    private Vector2Int serializedGridDimensions;

    public Vector2 min;    
    public Vector2 max;
    public Vector2 cellSize;

    public GridCell[,] FoundationGrid
    {
        get { return foundationGrid; }
    }

    public void SetFoundationGrid(GridCell[,] newFoundationGrid)
    {
        serializedGridDimensions = new Vector2Int(newFoundationGrid.GetLength(0), newFoundationGrid.GetLength(1));
        serializedGrid = new GridCell[serializedGridDimensions.x * serializedGridDimensions.y];
        for (int i = 0; i < serializedGridDimensions.x; ++i)
        {
            for (int j = 0; j < serializedGridDimensions.y; ++j)
            {
                serializedGrid[(i * serializedGridDimensions.y) + j] = newFoundationGrid[i, j];
            }
        }
    }

    public void Awake()
    {
        foundationGrid = new GridCell[serializedGridDimensions.x, serializedGridDimensions.y];
        for (int i = 0; i < serializedGridDimensions.x; ++i)
        {
            for (int j = 0; j < serializedGridDimensions.y; ++j)
            {
                foundationGrid[i, j] = serializedGrid[(i * serializedGridDimensions.y) + j];
            }
        }
    }

    public GridCell GetClosestCell(Vector3 Position)
    {
        Vector2 position2D = new Vector2(Position.x, Position.z);

        Vector2 cellPosition = (position2D - min) / cellSize;
        Vector2Int cellIntPosition = Vector2Int.zero;
        cellIntPosition.x = Mathf.Clamp(Mathf.RoundToInt(cellPosition.x), 0, foundationGrid.GetLength(0) - 1);
        cellIntPosition.y = Mathf.Clamp(Mathf.RoundToInt(cellPosition.y), 0, foundationGrid.GetLength(1) - 1);
        return foundationGrid[cellIntPosition.x, cellIntPosition.y];
    }

    public void Place(Foundation objectToPlace, GridCell placeOn, GridCell place)
    {

    }

    public bool CheckPlacement(Foundation checkWith)
    {
        return true;
    }
}
