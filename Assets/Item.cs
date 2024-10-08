using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item : MonoBehaviour
{

    public ItemType type;
    public float value;


}

public enum ItemType
{
    HandgunAmmo, ShotgunAmmo, RifleAmmo
}
