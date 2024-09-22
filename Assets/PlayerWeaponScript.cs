using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponScript : MonoBehaviour
{

    public static PlayerWeaponScript Instance;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public List<Weapon> weapons = new List<Weapon>();
    private bool justShoot = false;
    [SerializeField] private int equippedWeaponIndex = 0;


    private void Update()
    {
        if (Input.GetButtonDown("Shoot"))
        {
            justShoot = true;
            weapons[equippedWeaponIndex].Shoot();
        }

        if (Input.GetButtonUp("Shoot"))
        {
            justShoot = false;
            weapons[equippedWeaponIndex].StopShooting();
        }

        if (Input.GetButtonDown("Reload"))
        {
            weapons[equippedWeaponIndex].Reload();
        }

        if (Input.mouseScrollDelta.y > 0)
        {
            nextWeaponIndex();
        }
        else if (Input.mouseScrollDelta.y < 0)
        {
            prevWeaponIndex();
        }

    }

    private void nextWeaponIndex()
    {
        if (equippedWeaponIndex == weapons.Count - 1)
        {
            equippedWeaponIndex = 0;
        }
        else
        {
            equippedWeaponIndex++;
        }
    }

    private void prevWeaponIndex()
    {
        if (equippedWeaponIndex == 0)
        {
            equippedWeaponIndex = weapons.Count - 1;
        }
        else
        {
            equippedWeaponIndex--;
        }
    }

}
