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
    public Vector2Int offset;
    private PlayerAnimationControl player;

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

    public Tilemap wallTileMap;
    public Tilemap floorTileMap;

    private void Start()
    {
        player = PlayerAnimationControl.Instance;
    }

    private void InitializeGrid()
    {
        nodes = new Node[gridWidth, gridHeight];
        BoundsInt bounds = wallTileMap.cellBounds;
        offset = new Vector2Int(-bounds.xMin, -bounds.yMin);

        foreach (Vector3Int vector3Int in bounds.allPositionsWithin)
        {

            bool isWalkable = true;
            Sprite s = wallTileMap.GetSprite(vector3Int);
            Sprite s1 = floorTileMap.GetSprite(vector3Int);
            if (s != null || s1 == null)
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


    public Vector2 FindSpawnPointFromPlayer(int maxDistance, int minDistance)
    {
        Vector2[] allDirections = { Vector2.up, Vector2.down, Vector2.left, Vector2.right, new(1, 1), new(1, -1), new(-1, 1), new(-1, -1) };

        Shuffle(allDirections);

        Vector2 playerPos = player.playerPosition.position;
        playerPos.x = Mathf.FloorToInt(playerPos.x);
        playerPos.y = Mathf.FloorToInt(playerPos.y);

        for (int j = 0; j < allDirections.Length; j++)
        {
            int maximum = 0;
            for (int i = 0; i < maxDistance - minDistance; i++)
            {
                Vector2 spawn = playerPos + allDirections[j] * (minDistance + i);

                if (GetNodeFromWorldPosition(spawn).IsWalkable == false)
                {
                    maximum = i;
                    break;
                }

            }

            if(maximum > 0)
            {
                int index = Random.Range(minDistance, minDistance + maximum);
                return (playerPos + allDirections[j] * index);

            }
        }

        return Vector2.zero;
    }

    void Shuffle(Vector2[] array)
    {
        for (int i = array.Length - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);

            Vector2 temp = array[i];
            array[i] = array[randomIndex];
            array[randomIndex] = temp;
        }
    }


}
