using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{

    public ItemType type;
    public int value;


    public Item(ItemType type, int value)
    {
        this.type = type;
        this.value = value;
    }

}

public enum ItemType
{
    HandgunAmmo, ShotgunAmmo, RifleAmmo
}
