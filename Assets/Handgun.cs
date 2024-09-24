using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Handgun : Weapon
{
    private RaycastHit2D hit;

    [SerializeField] private Transform weaponPoint;
    [SerializeField] private LayerMask targetLayer;

    public override void Prep()
    {
        hit = Physics2D.Raycast(weaponPoint.position, weaponScript.direction, 100f, targetLayer);

        if(hit.collider != null)
        {
            weaponScript.line.SetPosition(0, weaponPoint.position);
            weaponScript.line.SetPosition(1, hit.point);
        }
        else
        {
            weaponScript.line.SetPosition(0, weaponPoint.position);
            weaponScript.line.SetPosition(1, (Vector2)weaponPoint.position + (Vector2)weaponScript.direction * 100f);
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

            playerAnimation.StopAnimation();

            
        }
    }

    
    public override void StopShooting()
    {
        //Nope
    }
}
