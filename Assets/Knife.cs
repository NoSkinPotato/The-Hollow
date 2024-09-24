using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Knife : Weapon
{
    public override void Reload()
    {
        //Nothing
    }

    public override void Prep()
    {
        
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
