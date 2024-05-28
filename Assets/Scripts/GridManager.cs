using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject cellPrefab;
    public int rows = 6;
    public int columns = 7;
    public float cellSpacing = 0f;


    void Start()
    {
        CreateGrid();
    }

    void CreateGrid()
    {
        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                // Calculate the position for each cell
                Vector3 position = new Vector3(x * cellSpacing, y * cellSpacing, 0);

                GameObject cell = Instantiate(cellPrefab, position, Quaternion.identity);
                // Name the cell 
                cell.name = $"Cell ({x},{y})";
                // Set the GridManager as the parent of the cell 
                cell.transform.parent = this.transform;
            }
        }
    }
}


/*public class GridManager : MonoBehaviour
{
    public GameObject cellPrefab;
    public int rows = 6;
    public int columns = 7;
    public float cellSpacing = -5f; 

    void Start()
    {
        CreateGrid();
    }

    void CreateGrid()
    {
        // Get the size of the cell prefab
        Renderer cellRenderer = cellPrefab.GetComponent<Renderer>();
        Vector3 cellSize = cellRenderer.bounds.size;
        float cellWidth = cellSize.x;
        float cellHeight = cellSize.y;

        // Calculate total grid size
        float gridWidth = columns * (cellWidth + cellSpacing) - cellSpacing;
        float gridHeight = rows * (cellHeight + cellSpacing) - cellSpacing;

        // Calculate starting position to center the grid within the camera view
        Vector3 startPosition = new Vector3(-gridWidth / 2 + cellWidth / 2, -gridHeight / 2 + cellHeight / 2, 0);

        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                // Calculate the position for each cell based on its size and spacing
                Vector3 position = startPosition + new Vector3(x * (cellWidth + cellSpacing), y * (cellHeight + cellSpacing), 0);

                GameObject cell = Instantiate(cellPrefab, position, Quaternion.identity);
                // Name the cell
                cell.name = $"Cell ({x},{y})";
                // Set the GridManager as the parent of the cell
                cell.transform.parent = this.transform;
            }
        }
    }
}*/