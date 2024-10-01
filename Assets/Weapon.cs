using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Weapon: MonoBehaviour
{
    protected PlayerAnimationControl playerAnimation;
    protected PlayerWeaponScript weaponScript;
    protected CameraControl cameraControl;

    protected void Start()
    {
        playerAnimation = PlayerAnimationControl.Instance;
        weaponScript = PlayerWeaponScript.Instance;
        cameraControl = CameraControl.Instance; 
    }

    public abstract void Prep();
    public abstract void Shoot();
    public abstract void StopShooting();
    public abstract void Reload();

}
