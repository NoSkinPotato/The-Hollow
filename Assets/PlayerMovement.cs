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
    private AudioManager audioManager;
    float horizontal, vertical;

    bool justRan = false;

    private float speedAdjuster;

    [SerializeField] private float maxStamina;
    [SerializeField] private float currStamina;
    [SerializeField] private float staminaRate;

    [SerializeField] private float minimumStamina;

    private bool staminaExhaustion = false;


    private void Start()
    {
        audioManager = AudioManager.Instance;
        animationControl = PlayerAnimationControl.Instance;
        weaponScript = PlayerWeaponScript.Instance;
    }

    private void Update()
    {
        if (weaponScript.playerState == PlayerState.OffControl || weaponScript.playerState == PlayerState.NoMovementControl)
        {
            return;
        }


        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");


        StaminaHandle();

        if (Input.GetKey(KeyCode.LeftShift) && (horizontal != 0 || vertical != 0) && staminaExhaustion == false)
        {
            Run();
        }else if (justRan == true)
        {
            if (audioManager.CheckPlaying("Breath1") == true)
            {
                audioManager.Stop("Breath1");
            }
            weaponScript.SetSoundSignal(SoundLevel.Silent);
            animationControl.playerAnimator.SetBool("Run", false);
            weaponScript.playerState = PlayerState.OnAllControl;
            justRan = false;
        }
    }


    private void FixedUpdate()
    {
        if (weaponScript.playerState == PlayerState.OffControl || weaponScript.playerState == PlayerState.NoMovementControl)
        {
            return;
        }
            

        if(weaponScript.playerState == PlayerState.OnAllControl)
        {
            MovementAnimation();
            Moving(movementSpeed);
        }
        else if (weaponScript.playerState == PlayerState.RunControl)
        {
            Moving(runningSpeed);
        }
            
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

        if (audioManager.CheckPlaying("Breath1") == false) {

            audioManager.Play("Breath1");
        }

        weaponScript.SetSoundSignal(SoundLevel.Medium);
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

    public void PlayAudio(string audio)
    {
        audioManager.Play(audio);
    }

    private void StaminaHandle()
    {
        if (weaponScript.playerState == PlayerState.RunControl)
        {

            if (currStamina > 0)
            {
                currStamina -= Time.deltaTime * staminaRate;
            }
            else
            {
                if (audioManager.CheckPlaying("Breath2") == false)
                {
                    audioManager.Play("Breath2");
                }
                staminaExhaustion = true;
            }
        }
        else
        {
            if (currStamina < maxStamina)
            {
                currStamina += Time.deltaTime * staminaRate;

                if (currStamina >= minimumStamina && staminaExhaustion == true)
                {
                    if (audioManager.CheckPlaying("Breath2"))
                    {
                        audioManager.Stop("Breath2");
                    }
                    staminaExhaustion = false;
                }
            }
            else
            {
                currStamina = maxStamina;
            }
        }
    }





}

