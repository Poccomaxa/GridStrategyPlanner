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
    private Vector2Int gridSize;

    public Vector2 min;    
    public Vector2 max;
    public Vector2 cellSize;

    public GridCell[,] FoundationGrid
    {
        get { return foundationGrid; }
    }

    public void SetFoundationGrid(GridCell[,] newFoundationGrid)
    {
        gridSize = new Vector2Int(newFoundationGrid.GetLength(0), newFoundationGrid.GetLength(1));
        serializedGrid = new GridCell[gridSize.x * gridSize.y];
        for (int i = 0; i < gridSize.x; ++i)
        {
            for (int j = 0; j < gridSize.y; ++j)
            {
                serializedGrid[(i * gridSize.y) + j] = newFoundationGrid[i, j];
            }
        }
    }

    public void Awake()
    {
        foundationGrid = new GridCell[gridSize.x, gridSize.y];
        for (int i = 0; i < gridSize.x; ++i)
        {
            for (int j = 0; j < gridSize.y; ++j)
            {
                foundationGrid[i, j] = serializedGrid[(i * gridSize.y) + j];
            }
        }
    }

    public void RotateCV()
    {

    }

    public Vector2Int GetClosestCellIndex(Vector3 Position)
    {
        Vector2 position2D = new Vector2(Position.x, Position.z);

        Vector2 cellPosition = (position2D - min) / cellSize;
        Vector2Int cellIntPosition = Vector2Int.zero;
        cellIntPosition.x = Mathf.Clamp(Mathf.RoundToInt(cellPosition.x), 0, foundationGrid.GetLength(0) - 1);
        cellIntPosition.y = Mathf.Clamp(Mathf.RoundToInt(cellPosition.y), 0, foundationGrid.GetLength(1) - 1);
        return cellIntPosition;
    }

    public Vector3 GetClosestCellCenter(Vector3 Position)
    {
        Vector2Int closestCellIndex = GetClosestCellIndex(Position);
        return new Vector3(closestCellIndex.x * cellSize.x + min.x, 0, closestCellIndex.y * cellSize.y + min.y);
    }

    public void CheckAndPlace(Foundation objectToPlace, Vector2Int indexPlace)
    {
        if (CheckPlacement(objectToPlace, indexPlace))
        {
            GameObject placedObject = GameObject.Instantiate(objectToPlace.gameObject);
            placedObject.transform.position = objectToPlace.transform.position;
            for (int i = 0; i < objectToPlace.gridSize.x; ++i)
            {
                for (int j = 0; j < objectToPlace.gridSize.y; ++j)
                {
                    if (objectToPlace.FoundationGrid[i, j])
                    {
                        FoundationGrid[indexPlace.x + i, indexPlace.y + i].occupied = true;
                    }
                }
            }
        }
    }

    public bool CheckPlacement(Foundation checkWith, Vector2Int indexPlace)
    {
        for (int i = 0; i < checkWith.gridSize.x; ++i)
        {
            for (int j = 0; j < checkWith.gridSize.y; ++j)
            {
                Vector2Int checkCell = new Vector2Int(indexPlace.x + i, indexPlace.y + j);
                if (checkCell.x < 0 || checkCell.x >= gridSize.x || checkCell.y < 0 || checkCell.y >= gridSize.y)
                {
                    Debug.Log("Out of bounds when placing!");
                    return false;
                }

                if (FoundationGrid[checkCell.x, checkCell.y] == null)
                {
                    Debug.Log("No foundation to place!");
                    return false;
                }

                if (FoundationGrid[checkCell.x, checkCell.y].occupied)
                {
                    Debug.Log("Occupied with other building!");
                    return false;
                }
            }
        }

        return true;
    }
}
