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

    [SerializeField] private float distance;

    private Camera mainCam;
    [HideInInspector]
    public Vector2 direction;

    public List<Weapon> weapons = new List<Weapon>();
    private bool justShoot = false;
    [SerializeField] private int equippedWeaponIndex = 0;
    private PlayerAnimationControl playerAnimation;
    private InventorySystem inventorySystem;
    bool onSwitch = false;

    private void Start()
    {
        inventorySystem = InventorySystem.Instance;
        playerAnimation = PlayerAnimationControl.Instance;
        mainCam = Camera.main;
        direction = transform.up;
    }

    private void Update()
    {
        if (inventorySystem.inventoryOpen == true)
            return;

        Aim();

        playerAnimation.playerAnimator.SetInteger("WeaponIndex", equippedWeaponIndex);

        weapons[equippedWeaponIndex].Prep();

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

        if (onSwitch == false)
        {
            WeaponSwitching();

        }
        

    }

    private void WeaponSwitching()
    {
        if (Input.mouseScrollDelta.y > 0)
        {
            nextWeaponIndex();
        }
        else if (Input.mouseScrollDelta.y < 0)
        {
            prevWeaponIndex();
        }
        else{

            if (Input.GetKeyDown(KeyCode.Alpha1)){

                SwitchWeapon(0);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                SwitchWeapon(1);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                SwitchWeapon(2);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                SwitchWeapon(3);
            }

        }
    }

    private void SwitchWeapon(int x)
    {
        if (weapons.Count - 1 >= x)
        {
            equippedWeaponIndex = x;
            StartCoroutine(SwitchTimer());
        }
           
    }

    private IEnumerator SwitchTimer()
    {
        onSwitch = true;
        yield return new WaitForSeconds(0.5f);
        onSwitch = false;

    }

    private void nextWeaponIndex()
    {
        if (equippedWeaponIndex == weapons.Count - 1)
        {
            SwitchWeapon(0);
        }
        else
        {
            SwitchWeapon(equippedWeaponIndex + 1);
        }
    }

    private void prevWeaponIndex()
    {
        if (equippedWeaponIndex == 0)
        {
            SwitchWeapon(weapons.Count - 1);
        }
        else
        {
            SwitchWeapon(equippedWeaponIndex - 1);
        }
    }

    private void Aim()
    {
        Vector2 mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);

        if (Vector2.Distance(mousePos, transform.position) > distance)
        {
            direction = (mousePos - (Vector2)transform.position).normalized;
        }
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

}
