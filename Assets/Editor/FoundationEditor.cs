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
                GridSize.x = Mathf.RoundToInt((Max.x - Min.x) / CellExample.size.x) + 1;
                GridSize.y = Mathf.RoundToInt((Max.y - Min.y) / CellExample.size.y) + 1;

                Debug.LogFormat("Size of grid is: ({0})", GridSize);

                GridCell[,] foundationGrid = new GridCell[GridSize.x, GridSize.y];

                for (int i = 0; i < foundationGrid.GetLength(0); ++i)
                {
                    for (int j = 0; j < foundationGrid.GetLength(1); ++j)
                    {
                        foundationGrid[i, j] = null;
                    }                    
                }

                foreach (GridCell Cell in Cells)
                {
                    Vector2Int CellCoordinates = new Vector2Int();
                    CellCoordinates.x = Mathf.RoundToInt((Cell.transform.position.x - Min.x) / Cell.size.x);
                    CellCoordinates.y = Mathf.RoundToInt((Cell.transform.position.z - Min.y) / Cell.size.y);

                    foundationGrid[CellCoordinates.x, CellCoordinates.y] = Cell;
                    Vector2 CorrectedPosition = Min + CellCoordinates * CellExample.size;
                    Cell.transform.position = new Vector3(CorrectedPosition.x, 0, CorrectedPosition.y);
                }

                string TextGrid = "\n";
                for (int i = 0; i < foundationGrid.GetLength(0); ++i)
                {                    
                    for (int j = 0; j < foundationGrid.GetLength(1); ++j)
                    {
                        TextGrid += foundationGrid[i, j] ? "X" : "_";
                    }
                    TextGrid += "\n";
                }                
                Debug.Log(TextGrid);

                foundation.SetFoundationGrid(foundationGrid);

                EditorUtility.SetDirty(target);
            }
        }
    }
}
