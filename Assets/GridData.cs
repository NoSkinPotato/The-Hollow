using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEditor.PlayerSettings;

public class GridData : MonoBehaviour
{

    public static GridData Instance { get; private set; }
    public GameObject game;
    public List<Sprite> sprites = new List<Sprite>();
    [HideInInspector]
    public Vector2Int offset;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }



        InitializeGrid();
    }



    public int gridWidth = 10; 
    public int gridHeight = 10;
    public Vector2Int gridOrigin;  
    public float cellSize = 1f;

    public Node[,] nodes;

    public Tilemap tileMap;

    private void InitializeGrid()
    {
        nodes = new Node[gridWidth, gridHeight];
        BoundsInt bounds = tileMap.cellBounds;
        offset = new Vector2Int(-bounds.xMin, -bounds.yMin);

        foreach (Vector3Int vector3Int in bounds.allPositionsWithin)
        {

            bool isWalkable = true;
            Sprite s = tileMap.GetSprite(vector3Int);
            if (s != null)
            {
                isWalkable = false; 
            }

            Vector2Int nodePos = (Vector2Int)vector3Int + offset;

            nodes[nodePos.x, nodePos.y] = new Node((Vector2Int)vector3Int, isWalkable);
        }


        Debug.Log("Grid initialized with " + (gridWidth * gridHeight) + " nodes.");
    }


    public Node GetNodeFromWorldPosition(Vector2 worldPosition)
    {
        Vector2 pos = worldPosition + offset;
        int x = Mathf.FloorToInt(pos.x / cellSize);
        int y = Mathf.FloorToInt(pos.y / cellSize);

        return nodes[x, y];
    }


}
