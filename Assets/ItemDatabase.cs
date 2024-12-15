using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ItemDatabase : ScriptableObject
{
    public List<ItemStats> itemDatabase = new List<ItemStats> ();
    public int MaxInventorySlot = 10;
}

[System.Serializable]
public class ItemStats
{
    public ItemType type;
    public int maxValue;
    public string name;
    public int ValuePerAmount;
    public bool OnHealth;
    public int SpawnChance;
    public int rank;
}
