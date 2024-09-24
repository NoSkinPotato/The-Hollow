using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Weapon: MonoBehaviour
{
    protected PlayerAnimationControl playerAnimation;
    protected PlayerWeaponScript weaponScript;

    protected void Start()
    {
        playerAnimation = PlayerAnimationControl.Instance;
        weaponScript = PlayerWeaponScript.Instance;
    }

    public abstract void Prep();
    public abstract void Shoot();
    public abstract void StopShooting();
    public abstract void Reload();

}
