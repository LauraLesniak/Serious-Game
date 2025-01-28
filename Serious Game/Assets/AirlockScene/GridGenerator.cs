using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    public int gridWidth = 5;      // Number of tiles along the X-axis
    public int gridHeight = 5;     // Number of tiles along the Z-axis
    public float tileSize = 1.0f;  // Distance between the centers of each tile
    [Range(0.1f, 1.0f)]
    public float quadSize = 0.9f;  // Scale factor for each quad (0.9 = slight spacing)

    public Vector2Int startGridPosition = new Vector2Int(0, 0); // Starting position for the robot
    public Vector2Int finishGridPosition = new Vector2Int(0, 0); // Starting position for the robot

    public List<Vector2Int> obstaclePositions = new List<Vector2Int>(); // List of obstacle positions

    void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        // Calculate the offset to center the grid
        Vector3 offset = new Vector3(
            (gridWidth - 1) * tileSize * 0.5f,
            0,
            (gridHeight - 1) * tileSize * 0.5f
        );

        for (int x = 0; x < gridWidth; x++)
        {
            for (int z = 0; z < gridHeight; z++)
            {
                GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);

                Vector2Int gridPosition = new Vector2Int(x, z);

                if (gridPosition == startGridPosition)
                {
                    quad.GetComponent<Renderer>().material.color = Color.green;
                }
                else if (gridPosition == finishGridPosition)
                {
                    quad.GetComponent<Renderer>().material.color = Color.red;
                }
                else if (obstaclePositions.Contains(gridPosition))
                {
                    quad.GetComponent<Renderer>().material.color = Color.black; // Mark obstacles as black
                }
                else
                {
                    quad.GetComponent<Renderer>().material.color = Color.white;
                }

                // Position the quad with centering offset
                Vector3 position = new Vector3(x * tileSize, 0, z * tileSize) - offset;
                quad.transform.position = transform.position + position;

                quad.transform.rotation = Quaternion.Euler(90, 0, 0); // Rotate to lay flat

                // Scale the quad based on tileSize and quadSize for spacing
                quad.transform.localScale = Vector3.one * tileSize * quadSize;

                quad.transform.parent = this.transform; // Optional: keeps the hierarchy clean
            }
        }
    }

    public Vector3 GetPositionFromGrid(Vector2Int gridPosition)
    {
        Vector3 offset = new Vector3(
            (gridWidth - 1) * tileSize * 0.5f,
            0,
            (gridHeight - 1) * tileSize * 0.5f
        );

        Vector3 position = new Vector3(gridPosition.x * tileSize, 0, gridPosition.y * tileSize) - offset;
        return position;
    } 
}
