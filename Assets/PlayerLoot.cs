using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLoot : MonoBehaviour
{
    private InventorySystem inventorySystem;
    private UIManager UIManager;

    private void Start()
    {
        inventorySystem = InventorySystem.Instance;
        UIManager = UIManager.Instance;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Loot"))
        {
            Debug.Log("Loot");
            ItemContainer itemContainer = collision.gameObject.GetComponent<ItemContainer>();
            if (itemContainer.justDropped)
                return;

            if (inventorySystem.Loot(itemContainer.containedItem))
            {
                Destroy(collision.gameObject);

            }

        }else if (collision.gameObject.CompareTag("Note"))
        {
            UIManager.LootNote();
            Destroy(collision.gameObject);
        }

    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Loot"))
        {
            ItemContainer itemContainer = collision.gameObject.GetComponent<ItemContainer>();

            if (itemContainer.justDropped)
            {
                itemContainer.justDropped = false;
            }

        }
    }

}
