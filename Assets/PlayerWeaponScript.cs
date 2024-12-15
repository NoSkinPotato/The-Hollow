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
    public PlayerState playerState;

    public List<Weapon> weapons = new List<Weapon>();
    private bool justShoot = false;
    public int equippedWeaponIndex = 0;
    private PlayerAnimationControl playerAnimation;
    private InventorySystem inventorySystem;
    bool onSwitch = false;

    [HideInInspector]
    public float knifeDamage;
    public int currMagazine;


    private void Start()
    {

        playerState = PlayerState.OnAllControl;

        inventorySystem = InventorySystem.Instance;
        playerAnimation = PlayerAnimationControl.Instance;
        mainCam = Camera.main;
        direction = transform.up;
    }

    private void Update()
    {
        if (playerState == PlayerState.RunControl || playerState == PlayerState.OffControl)
            return;

        Aim();

        if (onSwitch == true) return;

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

        WeaponSwitching();


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

    public void pushCurrMagazine(int curr)
    {
        currMagazine = curr;
    }

    private void SwitchWeapon(int x)
    {
        if (weapons.Count - 1 >= x)
        {
            equippedWeaponIndex = x;
        }
           
        StartCoroutine(UIManager.Instance.TriggerWeaponWheel(x));
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


    public void ReloadWeapon(int weaponIndex)
    {
        weapons[weaponIndex].FillMagazine();
    }

    public void OnSwitchControl(int x)
    {
        if (x == 0) onSwitch = false;
        else onSwitch = true;
    }

    public void DamageEnemy(Collider2D collision, float damage)
    {
        if (!collision.gameObject.CompareTag("Enemy")) return;


        EnemyStatistics stats = collision.GetComponentInChildren<EnemyStatistics>();
        if (stats != null) {
            stats.DamageEnemy(damage);
        }
        else
        {
            Debug.Log("Stats script not found");
        }
    }

    public void ReloadSoundWeapon(int weaponIndex) {

        weapons[weaponIndex].ReloadSound();
    }


    public string GenerateWeaponInMag()
    {
        string temp = "";

        for (int i = 1; i < weapons.Count; i++)
        {
            if (i > 1) temp += "-";
            temp += weapons[i].currMagazine.ToString();
        }

        return temp;
    }

    public void FillWeaponsMag(string codes)
    {
        if (codes.Length == 0) return;

        Debug.Log("Decrypting " + codes);
        string[] ammoCounts = codes.Split('-');

        for (int i = 1; i < weapons.Count; i++)
        {
            weapons[i].currMagazine = int.Parse(ammoCounts[i - 1]);
        }

    }
}

public enum PlayerState
{
    OnAllControl, OffControl, RunControl, NoMovementControl
}
