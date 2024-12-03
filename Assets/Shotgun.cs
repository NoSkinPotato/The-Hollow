using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using static UnityEngine.GraphicsBuffer;

public class Shotgun : Weapon
{
    private RaycastHit2D hit;

    [SerializeField] private Transform weaponPoint;
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private List<LineRenderer> lineRenderers = new List<LineRenderer>();
    [SerializeField] private float visualSeconds;
    [SerializeField] private float spreadAngle = 20;
    [SerializeField] private Light2D weaponLight;
    [SerializeField] private SpriteRenderer fireEffects;
    [SerializeField] private float shakeDistance;
    [SerializeField] private float shakeStrength;
    [SerializeField] private float shakeDuration;

    [SerializeField] private int currMagazine;
    [SerializeField] private int maxMagazine;

    bool onReload = false;

    public override void Prep()
    {
       //nothing yet
    }

    public override void Reload()
    {
        Item shotgunAmmo = inventorySystem.ItemsInInventory.Find(x => x.type == ItemType.ShotgunAmmo && x.value > 0);
        if (shotgunAmmo == null)
            return;

        onReload = true;
        playerAnimation.PlayAnimation("ActionIndex", 3);
    }

    public override void FillMagazine()
    {
        int ammoInInventory = inventorySystem.CountItemsByType(ItemType.ShotgunAmmo);
        
        if (currMagazine < maxMagazine)
        {
            currMagazine++;

            if(currMagazine == maxMagazine)
                playerAnimation.EndAnimation();
        }


        inventorySystem.UseItem(ItemType.ShotgunAmmo, 1);
    }

    public override void Shoot()
    {
        if (currMagazine <= 0)
            return;


        if (playerAnimation.GetStopAnimation() == false || onReload)
        {
            onReload = false;
            playerAnimation.playerAnimator.SetInteger("ActionIndex", 2);
            ShootLogic();

            playerAnimation.StopAnimation();
        }
    }

    private Vector2 GetSpreadDirection(float angle)
    {
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

        float angleStep = spreadAngle / (lineRenderers.Count - 1); 

        float startAngle = -spreadAngle / 2;

        for (int i = 0; i < lineRenderers.Count; i++)
        {
            float currentAngle = startAngle + (i * angleStep);

            Vector2 shootDirection = GetSpreadDirection(currentAngle);

            RaycastHit2D hit = Physics2D.Raycast(weaponPoint.position, shootDirection, 50f, targetLayer);
            lineRenderers[i].SetPosition(0, weaponPoint.position);
            if (hit.collider != null)
            {
                weaponScript.DamageEnemy(hit.collider, WeaponDamage);
                lineRenderers[i].SetPosition(1, hit.point);
            }
            else
            {
                lineRenderers[i].SetPosition(1, (Vector2)weaponPoint.position + shootDirection * 50f);
            }

            StartCoroutine(ShootVisual(visualSeconds, lineRenderers[i]));

            
        }

        cameraControl.Shake(-weaponScript.direction, shakeDuration, shakeDistance, shakeStrength);
    }

    private IEnumerator ShootVisual(float seconds, LineRenderer line)
    {
        line.enabled = true;
        weaponLight.enabled = true;
        fireEffects.enabled = true;
        yield return new WaitForSeconds(seconds);
        line.enabled = false;
        fireEffects.enabled = false;
        weaponLight.enabled = false;
    }

    public override void StopShooting()
    {
        //Nope
    }


}
