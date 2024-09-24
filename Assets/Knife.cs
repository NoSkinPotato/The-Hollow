using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Knife : Weapon
{
    public Knife()
    {
        weaponType = WeaponType.knife;
    }

    public override void Reload()
    {
        //Nothing
    }

    public override void Shoot()
    {
        if (playerAnimation == null)
        {
            Debug.Log("Player Animation Null");
            return;
        }


        if (playerAnimation.GetStopAnimation() == false)
        {
            playerAnimation.playerAnimator.SetInteger("ActionIndex", 2);

            playerAnimation.StopAnimation();
        }
    }

    public override void StopShooting()
    {
        //Nothing
    }
}
