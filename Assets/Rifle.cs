using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Rifle : Weapon
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
    [SerializeField] private float maxRecoil = 20;
    [Range(0f, .5f)]
    [SerializeField] private float recoilAcceleration;

    [SerializeField] private int currMagazine;
    [SerializeField] private int maxMagazine;


    private bool isShooting = false;
    private float recoil = 0;
    

    public override void Prep()
    {
        if (isShooting && recoil < maxRecoil)
        {
            recoil += Time.deltaTime * recoilAcceleration;

        }else if (isShooting == false && recoil > 0)
        {
            recoil -= Time.deltaTime * recoilAcceleration * 2;
        }

        if (isShooting == true && playerAnimation.GetStopAnimation() == false)
        {
            if (currMagazine <= 0)
                return;


            playerAnimation.playerAnimator.SetInteger("ActionIndex", 2);
            ShootLogic();
            playerAnimation.StopAnimation();
            
        }

    }

    public override void Reload()
    {
        Item rifleAmmo = inventorySystem.ItemsInInventory.Find(x => x.type == ItemType.RifleAmmo && x.value > 0);
        if (rifleAmmo == null)
            return;

        playerAnimation.PlayAnimation("ActionIndex", 3);
    }

    public override void FillMagazine()
    {
        int ammoInInventory = inventorySystem.CountItemsByType(ItemType.RifleAmmo);
        int difference = maxMagazine - currMagazine;

        if (ammoInInventory > difference)
        {
            currMagazine = maxMagazine;
        }
        else
        {
            currMagazine += ammoInInventory;
        }

        inventorySystem.UseItem(ItemType.RifleAmmo, difference);

    }
    public override void Shoot()
    {
        if (playerAnimation.GetStopAnimation() == false && isShooting == false)
        {
            isShooting = true;
        }
    }

    private Vector2 GetShotDirection(float angle)
    {
        int x = UnityEngine.Random.Range(0, 2);
        if (x == 0)
            angle *= -1;

        float radianAngle = angle * Mathf.Deg2Rad;

        Vector2 forward = weaponScript.direction;

        Vector2 spreadDir = new Vector2(
            forward.x * Mathf.Cos(radianAngle) - forward.y * Mathf.Sin(radianAngle),
            forward.x * Mathf.Sin(radianAngle) + forward.y * Mathf.Cos(radianAngle)
        );

        return spreadDir.normalized;
    }

    private void ShootLogic()
    {
        currMagazine--;

        Vector2 direction = GetShotDirection(recoil);

        hit = Physics2D.Raycast(weaponPoint.position, direction, 100f, targetLayer);
        lineRenderer.SetPosition(0, weaponPoint.position);
        if (hit.collider != null)
        {
            weaponScript.DamageEnemy(hit.collider, WeaponDamage);

            lineRenderer.SetPosition(1, hit.point);
        }
        else
        {
            lineRenderer.SetPosition(1, (Vector2)weaponPoint.position + direction * 100f);
        }

        StartCoroutine(ShootVisual(gunSeconds));

        cameraControl.Shake(direction, shakeDuration, shakeDistance, shakeStrength);
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
        isShooting = false;
    }


   
}
