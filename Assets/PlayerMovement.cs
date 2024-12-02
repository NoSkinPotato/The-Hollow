using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    [SerializeField] private float runningSpeed;
    [SerializeField] private Rigidbody2D rb;

    public float currentSpeed;

    private PlayerAnimationControl animationControl;
    private PlayerWeaponScript weaponScript;
    float horizontal, vertical;

    bool justRan = false;

    private float speedAdjuster;

    private void Start()
    {
        animationControl = PlayerAnimationControl.Instance;
        weaponScript = PlayerWeaponScript.Instance;
    }

    private void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");



        if (Input.GetKey(KeyCode.LeftShift) && (horizontal != 0 || vertical != 0))
        {
            Run();
        }else if (justRan == true)
        {
            animationControl.playerAnimator.SetBool("Run", false);
            weaponScript.playerState = PlayerState.OnAllControl;
            justRan = false;

        }

    }
    private void FixedUpdate()
    {
        if (weaponScript.playerState == PlayerState.OffControl || weaponScript.playerState == PlayerState.NoMovementControl)
            return;

        if(weaponScript.playerState == PlayerState.OnAllControl)
        {
            MovementAnimation();
            Moving(movementSpeed);
        }
        else if (weaponScript.playerState == PlayerState.RunControl)
            Moving(runningSpeed);
    }

    private void Moving(float speed)
    {
        currentSpeed = speed;
        rb.position += new Vector2(horizontal, vertical).normalized * Time.fixedDeltaTime * speed;
    }

    private void MovementAnimation()
    {
        if (Mathf.Abs(horizontal) > 0 || Mathf.Abs(vertical) > 0)
        {
            animationControl.PlayAnimation("ActionIndex", 1);
        }
        else
        {
            animationControl.PlayAnimation("ActionIndex", 0);
        }
    }

    private void Run()
    {
        if (animationControl.GetStopAnimation())
            return;


        weaponScript.playerState = PlayerState.RunControl;
        Vector2 direction = new Vector2(horizontal, vertical).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
        animationControl.playerAnimator.SetBool("Run", true);
        justRan = true;
    }

    public void StopMovement()
    {
        weaponScript.playerState = PlayerState.NoMovementControl;
    }

    public void NormalMovement()
    {
        weaponScript.playerState = PlayerState.OnAllControl;
    }
}

