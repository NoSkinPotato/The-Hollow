using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;

public class EnemyPathfinder : MonoBehaviour
{

    private GridData gridManager;
    public bool StopPathFinding = true;
    public bool isMoving = false;
    public EnemyAI enemyScript;
    Vector2 offset = new Vector2(0.5f, 0.5f);

    private void Start()
    {
        gridManager = GridData.Instance;
    }


    private void Update()
    {
        if (!isMoving && StopPathFinding == false)
        {
            isMoving = true;
            List<Node> path = FindPath((Vector2)transform.position, enemyScript.playerPosition);

            if (path != null)
            {

                StartCoroutine(WalkThePath(path));

            }
        }
    }

    public void StopPF()
    {
        if (!StopPathFinding)
            StopPathFinding = true;
    }

    public void StartPF()
    {
        if (StopPathFinding)
        {
            StopPathFinding = false;
            isMoving = false;
        }
    }

    private IEnumerator WalkThePath(List<Node> path)
    {
        int currNodeIndex = 0;

        while (currNodeIndex < path.Count)
        {
            Vector2 nextPos = path[currNodeIndex].position;

            while (gridManager.GetNodeFromWorldPosition((Vector2)transform.position) != path[currNodeIndex])
            {
                if (StopPathFinding == true) yield break;

                Vector2 direction = (nextPos + offset - (Vector2)transform.position).normalized;
                enemyScript.rb.transform.position += (Vector3)direction * enemyScript.movementSpeed/3f * Time.fixedDeltaTime;

                yield return null;
            }
            currNodeIndex++;
        }

        StopPathFinding = true;
        isMoving = false;
    }

    public List<Node> FindPath(Vector2 startPos, Vector2 targetPos)
    {
        Node startNode = gridManager.GetNodeFromWorldPosition(startPos);
        Node targetNode = gridManager.GetNodeFromWorldPosition(targetPos);

        if (startNode == null || targetNode == null)
            return null; 

        List<Node> openList = new List<Node>();
        HashSet<Node> closedList = new HashSet<Node>();

        openList.Add(startNode);

        while (openList.Count > 0)
        {
            Node currentNode = GetLowestFCostNode(openList);

            if (currentNode == targetNode)
                return RetracePath(startNode, targetNode);

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach (Node neighbor in GetNeighbors(currentNode))
            {
                if (!neighbor.IsWalkable || closedList.Contains(neighbor))
                    continue; 

                int newGCost = currentNode.GCost + GetDistance(currentNode, neighbor);
                if (newGCost < neighbor.GCost || !openList.Contains(neighbor))
                {
                    neighbor.GCost = newGCost;
                    neighbor.HCost = GetDistance(neighbor, targetNode);
                    neighbor.parent = currentNode;

                    if (!openList.Contains(neighbor))
                        openList.Add(neighbor);
                }
            }
        }

        return null;
    }

    private List<Node> RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();

        return path;
    }

    private List<Node> GetNeighbors(Node node)
    {
        List<Node> neighbors = new List<Node>();

        Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
        foreach (var direction in directions)
        {
            Vector2Int neighborPos = node.position + direction;
            Node neighbor = gridManager.GetNodeFromWorldPosition(neighborPos);
            if (neighbor != null)
                neighbors.Add(neighbor);
        }

        return neighbors;
    }

    private Node GetLowestFCostNode(List<Node> openList)
    {
        Node lowestCostNode = openList[0];
        foreach (Node node in openList)
        {
            if (node.FCost < lowestCostNode.FCost)
                lowestCostNode = node;
        }
        return lowestCostNode;
    }

    private int GetDistance(Node nodeA, Node nodeB)
    {
        int dx = Mathf.Abs(nodeA.position.x - nodeB.position.x);
        int dy = Mathf.Abs(nodeA.position.y - nodeB.position.y);
        return dx + dy; 
    }

}
