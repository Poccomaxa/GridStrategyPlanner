using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

[CustomEditor(typeof(Foundation))]
public class FoundationEditor : Editor
{
    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("CollectGrid"))
        {
            Foundation foundation = (Foundation)target;

            GridCell[] Cells = foundation.gameObject.GetComponentsInChildren<GridCell>();
            if (Cells.Length > 0)
            {
                GridCell CellExample = Cells.First();
                Vector2 Min, Max;
                Min.x = Cells.Min(gridCell => gridCell.transform.position.x);
                Min.y = Cells.Min(gridCell => gridCell.transform.position.z);
                Max.x = Cells.Max(gridCell => gridCell.transform.position.x);
                Max.y = Cells.Max(gridCell => gridCell.transform.position.z);

                Vector2Int GridSize = new Vector2Int();
                GridSize.x = Mathf.RoundToInt((Max.x - Min.x) / CellExample.Size.x) + 1;
                GridSize.y = Mathf.RoundToInt((Max.y - Min.y) / CellExample.Size.y) + 1;

                Debug.LogFormat("Size of grid is: ({0})", GridSize);

                foundation.FoundationGrid = new bool[GridSize.x, GridSize.y];

                for (int i = 0; i < foundation.FoundationGrid.GetLength(0); ++i)
                {
                    for (int j = 0; j < foundation.FoundationGrid.GetLength(1); ++j)
                    {
                        foundation.FoundationGrid[i, j] = false;
                    }                    
                }

                foreach (GridCell Cell in Cells)
                {
                    Vector2Int CellCoordinates = new Vector2Int();
                    CellCoordinates.x = Mathf.RoundToInt((Cell.transform.position.x - Min.x) / Cell.Size.x);
                    CellCoordinates.y = Mathf.RoundToInt((Cell.transform.position.z - Min.y) / Cell.Size.y);

                    foundation.FoundationGrid[CellCoordinates.x, CellCoordinates.y] = true;
                }

                string TextGrid = "\n";
                for (int i = 0; i < foundation.FoundationGrid.GetLength(0); ++i)
                {                    
                    for (int j = 0; j < foundation.FoundationGrid.GetLength(1); ++j)
                    {
                        TextGrid += foundation.FoundationGrid[i, j] ? "X" : "_";
                    }
                    TextGrid += "\n";
                }
                Debug.Log(TextGrid);
            }
        }
    }
}
