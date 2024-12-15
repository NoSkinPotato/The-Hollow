using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public Vector2Int position;
    public bool IsWalkable;     
    public int GCost;          
    public int HCost;          
    public int FCost => GCost + HCost; 
    public Node parent;
    public bool hasObject;

    public Node(Vector2Int position, bool isWalkable)
    {
        this.position = position;
        this.IsWalkable = isWalkable;
        this.GCost = int.MaxValue;
        this.HCost = 0;
        this.parent = null;
    }

}
