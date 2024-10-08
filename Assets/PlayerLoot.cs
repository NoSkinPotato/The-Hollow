using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLoot : MonoBehaviour
{
    private InventorySystem inventorySystem = InventorySystem.Instance;



    private void OnTriggerEnter2D(Collider2D collision)
    {
        inventorySystem.Loot(collision.GetComponent<Item>());
    }

}
