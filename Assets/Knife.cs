using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Knife : Weapon
{
    bool setKnifeDamage = false;

    public override void Reload()
    {
        //Nothing
    }

    public override void Prep()
    {
        if (!setKnifeDamage)
        {
            weaponScript.knifeDamage = WeaponDamage;
            setKnifeDamage = true;
        }

    }

    public override void FillMagazine()
    {
        throw new System.NotImplementedException();
        
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
        //Nothing
    }
}
