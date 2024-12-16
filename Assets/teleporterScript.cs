using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teleporterScript : MonoBehaviour
{

    public RectTransform UIInteraction;

    private bool touch = false;

    public GameObject numPad;

    
    private PlayerWeaponScript playerWeaponScript;
    private InventorySystem inventorySystem;
    [SerializeField] private NumPadScript numPadScript; 

    private void Start()
    {
        playerWeaponScript = PlayerWeaponScript.Instance;
        inventorySystem = InventorySystem.Instance;
    }

    private void Update()
    {
        if (touch && Input.GetKeyDown(KeyCode.F))
        {
            TriggerNumpad();
        }
    }

    private void TriggerNumpad()
    {
        if (numPad.gameObject.activeSelf)
        {
            
            numPad.gameObject.SetActive(false);
            playerWeaponScript.playerState = PlayerState.OnAllControl;
        }
        else
        {
            numPad.gameObject.SetActive(true);
            playerWeaponScript.playerState = PlayerState.OffControl;
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            inventorySystem.noInventoryUse = true;
            UIInteraction.gameObject.SetActive(true);
            touch = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerWeaponScript.playerState = PlayerState.OnAllControl;

            inventorySystem.noInventoryUse = false;

            if (UIInteraction != null)
            {
                UIInteraction.gameObject.SetActive(false);
            }
            
            if (numPadScript != null)
            {
                numPadScript.transform.GetChild(0).gameObject.SetActive(false);
            }

            
            touch = false;
        }
    }

}
