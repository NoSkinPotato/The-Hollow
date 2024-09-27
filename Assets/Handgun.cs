using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Handgun : Weapon
{
    private RaycastHit2D hit;

    [SerializeField] private Transform weaponPoint;
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private float gunSeconds;

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
        playerAnimation.PlayAnimation("ActionIndex", 3);
    }
    
    public override void Shoot()
    {
        if (playerAnimation.GetStopAnimation() == false)
        {
            playerAnimation.playerAnimator.SetInteger("ActionIndex", 2);
            ShootLogic();

            playerAnimation.StopAnimation();
        }
    }

    private void ShootLogic()
    {
        if (EnoughAmmo() == false)
        {

            return;
        }
            
        StartCoroutine(ShootVisual(gunSeconds));



    }

    private IEnumerator ShootVisual(float seconds)
    {
        lineRenderer.enabled = true;
        yield return new WaitForSeconds(seconds);
        lineRenderer.enabled = false;
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
