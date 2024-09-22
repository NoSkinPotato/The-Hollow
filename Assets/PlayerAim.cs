using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    [SerializeField] private float distance;
    [SerializeField] private LineRenderer line;
    [SerializeField] private Transform weaponPoint;

    private Camera mainCam;
    private Vector2 direction;

    private void Start()
    {
        mainCam = Camera.main;
        direction = transform.up;
    }

    private void Update()
    {
        Vector2 mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);

        if (Vector2.Distance(mousePos, transform.position) > distance)
        {
            direction = (mousePos - (Vector2)transform.position).normalized;
        }

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0f, 0f, angle);

        


    }
}
