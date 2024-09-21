using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    [SerializeField] private Rigidbody2D rb;

    private Camera mainCam;

    private void Start()
    {
        mainCam = Camera.main;
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
    }
}
