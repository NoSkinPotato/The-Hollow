using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Weapon: MonoBehaviour
{
    public WeaponType weaponType;
    protected PlayerAnimationControl playerAnimation;
    protected PlayerWeaponScript weaponScript;

    protected virtual void Awake()
    {
        playerAnimation = PlayerAnimationControl.Instance;
        weaponScript = PlayerWeaponScript.Instance;
    }

    public abstract void Shoot();
    public abstract void StopShooting();
    public abstract void Reload();

}

public enum WeaponType
{
    knife,
    handgun,
    shotgun,
    rifle
}
