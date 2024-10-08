using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{

    public static InventorySystem Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    [SerializeField] private List<Item> ItemsInInventory = new List<Item>();



    public bool Loot(Item item)
    {
        //CheckForInventory
        if (CheckForSpace(item) == false)
            return false;


        ItemsInInventory.Add(item);

        return true;
    }

    public void GetItem(ItemType type, int value)
    {
        



    }

    private bool CheckForSpace(Item item)
    {
        return false;
    }






}
