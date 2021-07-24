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
        IEnumerable <GridCell> enumerableGrid = foundationGrid.Cast<GridCell>();
        float minDistance = enumerableGrid.Min(gridCell =>
        {
            if (gridCell != null)
            {
                return (gridCell.transform.position - Position).magnitude;
            }
            else
            {
                return float.MaxValue;
            }
        });
        return enumerableGrid.First(gridCell =>
        {
            if (gridCell != null)
            {
                return (gridCell.transform.position - Position).magnitude <= minDistance;
            }
            else
            {
                return false;
            }
        });
    }
}
