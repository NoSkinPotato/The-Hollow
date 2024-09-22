using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    [SerializeField] private Rigidbody2D rb;

    private PlayerAnimationControl animationControl;

    private Camera mainCam;

    private void Start()
    {
        mainCam = Camera.main;
        animationControl = PlayerAnimationControl.Instance;
    }

    void Update()
    {
        mainCam.transform.position = (Vector2)transform.position;

    }

    private void FixedUpdate()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        rb.position += new Vector2(horizontal, vertical).normalized * Time.fixedDeltaTime * movementSpeed;

        MovementAnimation(horizontal, vertical);

    }

    private void MovementAnimation(float h, float v)
    {
        if (Mathf.Abs(h) > 0 || Mathf.Abs(v) > 0)
        {
            animationControl.PlayAnimation("ActionIndex", 1);
        }
        else
        {
            animationControl.PlayAnimation("ActionIndex", 0);
        }
    }
}
