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
                foundation.min.x = Cells.Min(gridCell => gridCell.transform.position.x);
                foundation.min.y = Cells.Min(gridCell => gridCell.transform.position.z);
                foundation.max.x = Cells.Max(gridCell => gridCell.transform.position.x);
                foundation.max.y = Cells.Max(gridCell => gridCell.transform.position.z);
                foundation.cellSize = Cells.First().size; 

                Vector2Int GridSize = new Vector2Int();
                GridSize.x = Mathf.RoundToInt((foundation.max.x - foundation.min.x) / foundation.cellSize.x) + 1;
                GridSize.y = Mathf.RoundToInt((foundation.max.y - foundation.min.y) / foundation.cellSize.y) + 1;

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
                    CellCoordinates.x = Mathf.RoundToInt((Cell.transform.position.x - foundation.min.x) / Cell.size.x);
                    CellCoordinates.y = Mathf.RoundToInt((Cell.transform.position.z - foundation.min.y) / Cell.size.y);

                    foundationGrid[CellCoordinates.x, CellCoordinates.y] = Cell;
                    Vector2 CorrectedPosition = foundation.min + CellCoordinates * foundation.cellSize;
                    Cell.transform.position = new Vector3(CorrectedPosition.x, 0, CorrectedPosition.y);
                }

                //string TextGrid = "\n";
                //for (int i = 0; i < foundationGrid.GetLength(0); ++i)
                //{                    
                //    for (int j = 0; j < foundationGrid.GetLength(1); ++j)
                //    {
                //        TextGrid += foundationGrid[i, j] ? "X" : "_";
                //    }
                //    TextGrid += "\n";
                //}                
                //Debug.Log(TextGrid);

                foundation.SetFoundationGrid(foundationGrid);

                EditorUtility.SetDirty(target);
            }
        }
    }
}
