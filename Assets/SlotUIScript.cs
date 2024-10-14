using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UI;

public class SlotUIScript : MonoBehaviour
{
    [SerializeField] private Image slotImage;
    [SerializeField] private TextMeshProUGUI textMesh;
    [SerializeField] private TextMeshProUGUI amountMesh;

    public bool clicked = false;
    public Item containedItem;
    private InventorySystem inventorySystem;
    private RectTransform transform;

    private void Start()
    {
        inventorySystem = InventorySystem.Instance;
        transform = GetComponent<RectTransform>();
    }


    public void SetItem(Item item)
    {
        slotImage.fillCenter = true;
        textMesh.enabled = true;
        textMesh.text = item.type.ToString();
        amountMesh.enabled = true;
        amountMesh.text = item.value.ToString();
        containedItem = item;
    }

    public void RemoveItem()
    { 
        slotImage.fillCenter = false;
        textMesh.enabled = false;
        amountMesh.enabled = false;
        containedItem = null;
        clicked = false;
    }

    public void OnClick()
    {
        if(clicked == true)
        {
            inventorySystem.DeactivateDisplayInteraction();
            clicked = false;
            return;
        }



        if (containedItem != null)
        {

            inventorySystem.DisplayInteraction(Input.mousePosition, containedItem);
            clicked = true;
        }
        else
        {
            Debug.Log("Clicked Item Do not Contain an Item");
        }
        
    }



}
