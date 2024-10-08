using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLoot : MonoBehaviour
{
    private InventorySystem inventorySystem;

    private void Start()
    {
        inventorySystem = InventorySystem.Instance;
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (inventorySystem.Loot(collision.GetComponent<ItemContainer>().containedItem)){
            Destroy(collision.gameObject);

        }
    }

}
