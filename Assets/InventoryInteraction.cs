using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryInteraction : MonoBehaviour
{
    private Item interactedItem;
    [SerializeField] private GameObject useButton;
    [SerializeField] private GameObject dropButton;
    private InventorySystem inventorySystem;
    private AudioManager audioManager;
    private void Start()
    {
        inventorySystem = InventorySystem.Instance;
        audioManager = AudioManager.Instance;
    }

    public void InteractItem(Vector2 pos, Item item, bool usable)
    {
        interactedItem = item;

        if (!usable)
            useButton.SetActive(false);
        else
            useButton.SetActive(true);

        transform.position = pos;
    }


    public void OnUse()
    {
        audioManager.Play("Click");
        //IncreasePlayerHealth
        inventorySystem.HealWithItem(interactedItem);
        gameObject.SetActive(false);

    }

    public void OnDrop()
    {
        audioManager.Play("Click");
        inventorySystem.DropItem(interactedItem);
        gameObject.SetActive(false);
    }




}
