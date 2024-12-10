using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{

    public ItemType type;
    public int value;
    [HideInInspector]
    public string name;
    public bool useOnHealth;

    public Item(ItemType type,  int value, bool useOnHealth, string name)
    {
        this.type = type;
        this.value = value;
        this.useOnHealth = useOnHealth;
        this.name = name;
    }

    public Item(string name, int value)
    {
        this.name = name;
        this.value = value;
    }


}

public enum ItemType
{
    HandgunAmmo, ShotgunAmmo, RifleAmmo, Medkit, Bandage
}
