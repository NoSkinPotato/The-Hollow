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
    [SerializeField] private Dictionary<string, int> database = new Dictionary<string, int>();

    private int amountLooted;


    public bool Loot(Item item)
    {
        amountLooted = 0;
        /*
        while (InsertItem(item) == true) ;

        */

        //Add Notifications
        Debug.Log("Looted: " + amountLooted + "x " + item.type.ToString());
        return true;
    }

    public void GetItem(ItemType type, int value)
    {

    }
    /*
    private bool InsertItem(Item item)
    {
        if (InsertItem()) return true;

    }

    private bool InsertOnSpace(Item item)
    {

    }

    private bool InsertOnSlot(Item item)
    {

    }*/
}
