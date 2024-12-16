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

    public List<Item> ItemsInInventory = new List<Item>();
    [SerializeField] private ItemDatabase inventoryDatabase;
    [SerializeField] private Transform UIList;
    [SerializeField] private GameObject slotObject;
    [SerializeField] private InventoryInteraction inventoryDisplay;
    [SerializeField] private ItemContainer lootObject;
    [SerializeField] private RectTransform inventoryUI;
    [SerializeField] private float inventoryDuration = 0.5f;

    private List<SlotUIScript> allSlots = new List<SlotUIScript>();

    private int amountLooted;
    private PlayerAnimationControl playerScript;
    private PlayerWeaponScript playerWeaponScript;
    private AudioManager audioManager;
    private UIManager UImanager;
    private PlayerStatsScript statsScript;
    public bool inventoryOpen = false;
    public bool noInventoryUse = false;
    bool updatingInventory = false;

    private void Start()
    {
        playerScript = PlayerAnimationControl.Instance;
        playerWeaponScript = PlayerWeaponScript.Instance;
        UImanager = UIManager.Instance;
        statsScript = PlayerStatsScript.Instance;
        audioManager = AudioManager.Instance;

        //SyncWithUI

        StartSync();

    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && updatingInventory == false && playerScript.GetStopAnimation() == false && noInventoryUse == false)
        {
            if (inventoryOpen == false)
            {
                playerWeaponScript.playerState = PlayerState.OffControl;
            }
            else
            {
                playerWeaponScript.playerState = PlayerState.OnAllControl;
            }
                

            updatingInventory = true;
            inventoryOpen = !inventoryOpen;

            inventoryDisplay.gameObject.SetActive(false);

            playerScript.playerAnimator.SetBool("OnInventory", inventoryOpen);
            StartCoroutine(SetInventoryUI());
        }
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
            audioManager.Play("Loot");

            //Add Notifications
            string itemName = inventoryDatabase.itemDatabase.Find(x => x.type == item.type).name;

            UImanager.LootUI(new Item(itemName, amountLooted));

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
            Item newItem = new Item(item.type, item.value, item.useOnHealth, item.name);
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

    private IEnumerator SetInventoryUI()
    {
        Vector2 pos = inventoryUI.position;
        Vector2 originalPos = pos;

        pos.x *= -1;

        Vector2 targetPos = pos;

        float timeElapsed = 0;

        while (timeElapsed < inventoryDuration)
        {
            float t = timeElapsed / inventoryDuration;

            inventoryUI.position = Vector2.Lerp(originalPos, targetPos, t);

            timeElapsed += Time.deltaTime;



            yield return null;
        }


        inventoryUI.position = targetPos;
        updatingInventory = false;

    }


    public int CountItemsByType(ItemType type)
    {
        int x = 0;

        foreach(Item t in ItemsInInventory)
        {
            if(t.type == type)
            {
                x += t.value;
            }
        }

        return x;
    }

    public void UseItem(ItemType item, int value)
    {
        for (int i = 0; i < ItemsInInventory.Count; i++) {

            if (ItemsInInventory[i].type == item)
            {
                ItemsInInventory[i].value -= value;
                if (ItemsInInventory[i].value <= 0)
                {
                    value = Mathf.Abs(ItemsInInventory[i].value);
                    ItemsInInventory.RemoveAt(i);
                }
                else
                {
                    break;
                }

            }
        }

        SyncWithUI();
    }

    public void HealWithItem(Item item)
    {
        ItemStats stats = inventoryDatabase.itemDatabase.Find(x => x.type == item.type);

        if (stats != null)
        {
            int value = stats.ValuePerAmount;
            statsScript.HealPlayer(value);
            UseItem(item.type, 1);

        }
        else
        {
            Debug.Log("Inventory Data has not been inserted for Item Type : " + item.type);
        }
    }

}
