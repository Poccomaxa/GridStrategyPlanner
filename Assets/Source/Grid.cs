using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public GameObject GridCell;
    public Vector2Int Size;
    public bool[] GridValues;

    // Start is called before the first frame update
    void Start()
    {
        GridCell CellComponent = GridCell.GetComponent<GridCell>();
        Vector2 Start = -(CellComponent.size * Size) / 2;
        for (int x = 0; x < Size.x; ++x)
        {
            for (int y = 0; y < Size.y; ++y)
            {
                Vector3 Position = Start + CellComponent.size * new Vector2(x, y);
                Position.z = Position.y;
                Position.y = 0;
                GameObject.Instantiate(GridCell, Position, Quaternion.identity, transform);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
