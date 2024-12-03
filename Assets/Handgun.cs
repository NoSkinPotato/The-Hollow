using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;

public class Handgun : Weapon
{
    private RaycastHit2D hit;

    [SerializeField] private Transform weaponPoint;
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private float gunSeconds;
    [SerializeField] private Light2D weaponLight;
    [SerializeField] private SpriteRenderer fireEffects;
    [SerializeField] private float shakeDistance;
    [SerializeField] private float shakeStrength;
    [SerializeField] private float shakeDuration;

    [SerializeField] private int currMagazine;
    [SerializeField] private int maxMagazine;


    public override void Prep()
    {
        hit = Physics2D.Raycast(weaponPoint.position, weaponScript.direction, 100f, targetLayer);

        lineRenderer.SetPosition(0, weaponPoint.position);
        if (hit.collider != null)
        {
            lineRenderer.SetPosition(1, hit.point);
        }
        else
        {
            lineRenderer.SetPosition(1, (Vector2)weaponPoint.position + (Vector2)weaponScript.direction * 100f);
        }
    }

    public override void Reload()
    {
        Item handGunAmmo = inventorySystem.ItemsInInventory.Find(x => x.type == ItemType.HandgunAmmo && x.value > 0);
        if (handGunAmmo == null)
            return;

        playerAnimation.PlayAnimation("ActionIndex", 3);
    }

    public override void FillMagazine()
    {
        int ammoInInventory = inventorySystem.CountItemsByType(ItemType.HandgunAmmo);
        int difference = maxMagazine - currMagazine;


        if (ammoInInventory > difference)
        {
            currMagazine = maxMagazine;
        }
        else
        {
            currMagazine += ammoInInventory;
        }

        inventorySystem.UseItem(ItemType.HandgunAmmo, difference);

    }

    public override void Shoot()
    {
        if(currMagazine <= 0)
            return;

        if (hit.collider != null) { 
            weaponScript.DamageEnemy(hit.collider, WeaponDamage);
        
        }

        //CheckInventory
        if (playerAnimation.GetStopAnimation() == false)
        {
            currMagazine -= 1;
            playerAnimation.playerAnimator.SetInteger("ActionIndex", 2);
            ShootLogic();

            playerAnimation.StopAnimation();
        }
    }

    private void ShootLogic()
    {
        if (EnoughAmmo() == false) return;
            
        StartCoroutine(ShootVisual(gunSeconds));


        cameraControl.Shake(-weaponScript.direction, shakeDuration, shakeDistance, shakeStrength);
    }

    private IEnumerator ShootVisual(float seconds)
    {
        lineRenderer.enabled = true;
        weaponLight.enabled = true;
        fireEffects.enabled = true;
        yield return new WaitForSeconds(seconds);
        lineRenderer.enabled = false;
        fireEffects.enabled = false;
        weaponLight.enabled = false;
    }

    public override void StopShooting()
    {
        //Nope
    }


    private bool EnoughAmmo()
    {
        //Add Inventory stuff
        return true;
    }

}
