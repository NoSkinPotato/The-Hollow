using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyData
{
    public string name;
    public GameObject prefab;
    public float spawnChance;
    public int rank;
    public Vector2 size;
}
