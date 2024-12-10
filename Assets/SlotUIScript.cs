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

    [SerializeField] private Color noItemColor;
    [SerializeField] private Color itemColor;
    [SerializeField] private Color hoverColor;

    public bool clicked = false;
    public Item containedItem = null;
    private InventorySystem inventorySystem;

    private void Start()
    {
        inventorySystem = InventorySystem.Instance;
    }


    public void SetItem(Item item)
    {
        slotImage.fillCenter = true;
        textMesh.enabled = true;
        textMesh.text = item.type.ToString();
        amountMesh.enabled = true;
        amountMesh.text = item.value.ToString();
        containedItem = item;
        slotImage.color = itemColor;
    }

    public void RemoveItem()
    { 
        slotImage.fillCenter = false;
        textMesh.enabled = false;
        amountMesh.enabled = false;
        containedItem = null;
        clicked = false;
        slotImage.color = noItemColor;
    }

    public void OnClick()
    {

        if (containedItem == null)
            return;

        if (containedItem != null && containedItem.value != 0)
        {

            inventorySystem.DisplayInteraction(Input.mousePosition, containedItem);
            clicked = true;
        }
        else
        {
            Debug.Log("Clicked Item Do not Contain an Item");
        }
        
    }

    public void OnHover(bool x)
    {
        if (x && containedItem != null)
        {
            slotImage.color = hoverColor;

            textMesh.color = itemColor;
            amountMesh.color = itemColor;
        }
        else
        {
            textMesh.color = Color.white;
            amountMesh.color = Color.white;

            if (containedItem == null || containedItem.value == 0)
            {
                slotImage.color = noItemColor;
            }
            else
            {
                slotImage.color = itemColor;
            }

        }
        
    }



}
