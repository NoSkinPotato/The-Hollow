using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{

    public ItemType type;
    public int value;
    public bool useOnHealth;


    public Item(ItemType type, int value, bool useOnHealth)
    {
        this.type = type;
        this.value = value;
        this.useOnHealth = useOnHealth;
    }


}

public enum ItemType
{
    HandgunAmmo, ShotgunAmmo, RifleAmmo
}
