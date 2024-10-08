using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class doorScript : MonoBehaviour
{

    [SerializeField] private bool openDoor = false;
    [SerializeField] private bool closeDoor = true;

    private float originalAxis;
    private float axis;
    [SerializeField] private float doorOpenSpeed = 5f;

    [SerializeField] private Transform leftSide;
    [SerializeField] private Transform rightSide;

    [SerializeField] private bool allowDoor = true;

    private void Start()
    {
        originalAxis = transform.rotation.z;
        axis = originalAxis;

    }

    private void Update()
    {
        Vector3 euler = transform.rotation.eulerAngles;
        euler.z = axis;
        transform.rotation = Quaternion.Euler(euler);

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

            if(Vector2.Distance(collision.transform.position, leftSide.position) < Vector2.Distance(collision.transform.position, rightSide.position))
            {
                LeftDoorCollision();
            }
            else
            {
                RightDoorCollision();
            }
            
        }
    }

    private void RightDoorCollision()
    {
        Debug.Log("Right Collision");
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
        Debug.Log("Left Collision");
        if (openDoor == true && closeDoor == false)
        {
            closeDoor = true;
            return;
        }

        openDoor = false;
        closeDoor = false;
    }


}
