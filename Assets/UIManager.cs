using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public static UIManager Instance { get; private set; }

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


    private PlayerStatsScript playerStats;
    private PlayerWeaponScript playerWeapon;
    private InventorySystem inventorySystem;

    [SerializeField] private TextMeshProUGUI currMagText;
    [SerializeField] private TextMeshProUGUI AmmoInInventoryText;

    [SerializeField] private GameObject AmmoCounter;
    [SerializeField] private Image HealthScreen;
    [SerializeField] private float maxHealthScreenOpacity = 0.5f;

    [SerializeField] private float weaponWheelAutoPrompt = 1.5f;

    [SerializeField] private GameObject weaponWheel;
    [SerializeField] private List<Image> wheelWeapons;

    [SerializeField] private LootUIScript lootUIScript;
    

    private float wheelOpacity = 0.94f;

    private void Start()
    {
        playerStats = PlayerStatsScript.Instance;
        playerWeapon = PlayerWeaponScript.Instance;
        inventorySystem = InventorySystem.Instance;
    }

    private void Update()
    {

        HealthUI();
        AmmoUI();

    }

    private void AmmoUI()
    {
        currMagText.text = playerWeapon.currMagazine.ToString();

        ItemType weaponAmmoType = ItemType.HandgunAmmo;

        switch (playerWeapon.equippedWeaponIndex)
        {
            case 0:
                OnKnife();
                return;
            case 2:
                weaponAmmoType = ItemType.ShotgunAmmo;
                break;
            case 3:
                weaponAmmoType = ItemType.RifleAmmo;
                break;
        }

        AmmoCounter.SetActive(true);
        AmmoInInventoryText.text = inventorySystem.CountItemsByType(weaponAmmoType).ToString();
    }
    private void HealthUI()
    {
        float alpha = playerStats.GetCurrentPlayerHealth() / playerStats.GetPlayerMaxHealth();

        Color x = HealthScreen.color;
        x.a = maxHealthScreenOpacity - (alpha * maxHealthScreenOpacity);
        HealthScreen.color = x;
    }
    private void OnKnife()
    {
        AmmoCounter.SetActive(false);
    }

    public IEnumerator TriggerWeaponWheel(int weaponIndex)
    {
        weaponWheel.SetActive(true);
        Color color = wheelWeapons[weaponIndex].color;
        color.a = wheelOpacity;
        wheelWeapons[weaponIndex].color = color;

        yield return new WaitForSeconds(weaponWheelAutoPrompt);

        color = wheelWeapons[weaponIndex].color;
        color.a = 1;
        wheelWeapons[weaponIndex].color = color;
        weaponWheel.SetActive(false);

        yield return null;
    }

    public void LootUI(Item item)
    {
        lootUIScript.LootQueue.Add(item);

    }

}
