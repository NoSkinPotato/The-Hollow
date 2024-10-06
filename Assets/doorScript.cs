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

    [SerializeField] private Collider2D leftSideCollider;
    [SerializeField] private Collider2D rightSideCollider;

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
            Vector2 localCollisionPoint = collision.transform.position - transform.position;

            if ((int)axis == 0)
            {
                Debug.Log("Vertical");
                if (localCollisionPoint.x < 0)
                {
                    LeftDoorCollision();
                }
                else if (localCollisionPoint.x > 0)
                {
                    RightDoorCollision();
                }
            }
            else if((int)axis == 90)
            {
                Debug.Log("Horizontal");
                if (localCollisionPoint.y > 0)
                {
                    RightDoorCollision();
                }
                else if (localCollisionPoint.y < 0)
                {
                    
                    LeftDoorCollision();
                }

            }else if((int)axis == -90)
            {
                if (localCollisionPoint.y > 0)
                {
                    LeftDoorCollision(); 
                }
                else if (localCollisionPoint.y < 0)
                {
                    RightDoorCollision();
                }
            }else if ((int)axis == 180)
            {
                if (localCollisionPoint.x < 0)
                {
                    RightDoorCollision();
                   
                }
                else if (localCollisionPoint.x > 0)
                {
                    LeftDoorCollision();
                }
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
