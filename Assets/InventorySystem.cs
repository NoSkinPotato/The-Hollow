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
    [SerializeField] private ItemDatabase inventoryDatabase;

    private int amountLooted;


    public bool Loot(Item item)
    {
        amountLooted = 0;
        
        while (InsertItem(item) == true) ;

        
        if (amountLooted > 0)
        {
            //Add Notifications
            Debug.Log("Looted: " + amountLooted + "x " + item.type.ToString());

            if(item.value > 0)
            {
                return false;
            }

        }
        else
        {
            return false;
        }
        
        return true;
    }

    public void GetItem(ItemType type, int value)
    {

    }
    
    private bool InsertItem(Item item)
    {
        if (item.value == 0) return false;


        if (InsertOnSpace(item)) return true;


        return InsertOnSlot(item);
    }

    private bool InsertOnSpace(Item item)
    {
        foreach(Item i in ItemsInInventory)
        {
            if (i.type == item.type)
            {
                int max = MaxValueOf(i.type);

                if (i.value < max)
                {

                    int space = max - i.value;

                    if(space >= item.value)
                    {
                        i.value += item.value;
                        amountLooted += item.value;
                        item.value = 0;

                    }
                    else
                    {
                        Debug.Log("Space");
                        i.value += space;
                        item.value -= space;
                        amountLooted += space;
                    }


                    return true;
                }

            } 
        }

        return false;
    }

    

    private bool InsertOnSlot(Item item)
    {
        if(ItemsInInventory.Count < inventoryDatabase.MaxInventorySlot)
        {
            Item newItem = new Item(item.type, item.value);
            ItemsInInventory.Add(newItem);
            amountLooted += item.value;
            item.value = 0;
        }

        return false;
    }

    private int MaxValueOf(ItemType type)
    {
        return inventoryDatabase.itemDatabase.Find(x => x.type == type).maxValue;
    }
}
