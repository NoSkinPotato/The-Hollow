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
    [SerializeField] private Transform UIList;
    [SerializeField] private GameObject slotObject;
    [SerializeField] private InventoryInteraction inventoryDisplay;
    [SerializeField] private ItemContainer lootObject;

    private List<SlotUIScript> allSlots = new List<SlotUIScript>();

    private int amountLooted;
    private PlayerWeaponScript playerScript;

    private void Start()
    {
        playerScript = PlayerWeaponScript.Instance;

        //SyncWithUI

        StartSync();

    }

    private void StartSync()
    {
       

        //MaxInventory
        for (int i = 0; i < inventoryDatabase.MaxInventorySlot; i++)
        {

            GameObject slot = Instantiate(slotObject, UIList);
            SlotUIScript script = slot.GetComponent<SlotUIScript>();

            allSlots.Add(script);

            if (i < ItemsInInventory.Count)
            {
                script.SetItem(ItemsInInventory[i]);
            }
        }
    }

    private void SyncWithUI()
    {
        for (int i = 0; i < inventoryDatabase.MaxInventorySlot; i++)
        {
            if (i < ItemsInInventory.Count)
            {
                allSlots[i].SetItem(ItemsInInventory[i]);
            }
            else
            {
                allSlots[i].RemoveItem();

            }

        }
    }



    public bool Loot(Item item)
    {
        amountLooted = 0;

        while (InsertItem(item) == true) ;

        SyncWithUI();

        
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
            Item newItem = new Item(item.type, item.value, item.useOnHealth);
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


    public void DisplayInteraction(Vector2 pos, Item item)
    {
        inventoryDisplay.gameObject.SetActive(true);
        bool use = inventoryDatabase.itemDatabase.Find(x => x.type == item.type).OnHealth;
        inventoryDisplay.InteractItem(pos, item, use);

    }

    public void DeactivateDisplayInteraction()
    {
        inventoryDisplay.gameObject.SetActive(false);

    }

    public void DropItem(Item item)
    {
        ItemContainer newLoot = Instantiate(lootObject.gameObject).GetComponent<ItemContainer>();

        newLoot.transform.position = playerScript.transform.position;
        newLoot.justDropped = true;
        newLoot.containedItem = item;


        int index = ItemsInInventory.FindIndex(x => x == item);
        ItemsInInventory.RemoveAt(index);
        allSlots.Find(x => x.containedItem == item).RemoveItem();
    
    }

    public void CloseInventory()
    {

    }

    public void OpenInventory()
    {

    }

}
