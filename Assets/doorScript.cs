using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class doorScript : MonoBehaviour
{

    [SerializeField] private bool openDoor = false;
    [SerializeField] private bool closeDoor = true;

    private float originalAxis;
    private float axis;
    private float doorOpenSpeed = 5f;

    [SerializeField] private Transform leftSide;
    [SerializeField] private Transform rightSide;

    [SerializeField] private bool allowDoor = true;

    [SerializeField] private AudioSource doorCreak;
    [SerializeField] private AudioSource doorBreak;

    private bool playSound = false;

    private void Start()
    {
        originalAxis = transform.rotation.eulerAngles.z;
        axis = originalAxis;
    }

    private void Update()
    {
        Vector3 euler = transform.rotation.eulerAngles;
        euler.z = axis;
        transform.rotation = Quaternion.Euler(euler);

        if (allowDoor == false && playSound == false)
        {
            doorCreak.Play();
            playSound = true;
        }

        if (closeDoor)
        {
            CloseDoor();
        }
        else
        {
            if(openDoor!)
            {
                OpenDoorLeft();
            }
            else
            {
                OpenDoorRight();
            }

        }
            

    }

    private void OpenDoorRight()
    {
        
        if (axis < originalAxis + 90f)
        {
            allowDoor = false;
            axis += Time.deltaTime * doorOpenSpeed;
        }
        else
        {
            playSound = false;
            allowDoor = true;
        }
            

    }

    private void OpenDoorLeft()
    {
        if (axis > originalAxis - 90f)
        {
            allowDoor = false;
            axis -= Time.deltaTime * doorOpenSpeed;
        }
        else
        {
            playSound = false;
            allowDoor = true;
        }
            

    }

    private void CloseDoor()
    {
        
        if (openDoor == false)
        {
            if (axis > originalAxis)
            {
                allowDoor = false;
                axis -= Time.deltaTime * doorOpenSpeed;
            }
            else
            {
                
                playSound = false;
                allowDoor = true;
            }
        }
        else
        {
            if (axis < originalAxis)
            {
                allowDoor = false;
                axis += Time.deltaTime * doorOpenSpeed;

            }
            else
            {
                allowDoor = true;
            }

        }
        
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && allowDoor)
        {

            doorOpenSpeed = collision.gameObject.GetComponent<PlayerMovement>().currentSpeed * 25f;

            if (Vector2.Distance(collision.transform.position, leftSide.position) < Vector2.Distance(collision.transform.position, rightSide.position))
            {
                LeftDoorCollision();
            }
            else
            {
                RightDoorCollision();
            }

        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            doorBreak.Play();
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<ShadowCaster2D>().enabled = false;
        }
    }

    private void RightDoorCollision()
    {
        if (openDoor == false && closeDoor == false)
        {
            closeDoor = true;
            return;
        }

        openDoor = true;
        closeDoor = false;
    }
    private void LeftDoorCollision()
    {
        if (openDoor == true && closeDoor == false)
        {
            closeDoor = true;
            return;
        }

        openDoor = false;
        closeDoor = false;
    }

}
