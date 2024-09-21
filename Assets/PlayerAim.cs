using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    [SerializeField] private float offset;
    [SerializeField] private LineRenderer line;
    [SerializeField] private Transform weaponPoint;

    private void Update()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePos - (Vector2)transform.position).normalized;


        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0f, 0f, angle - offset);

        line.SetPosition(0, weaponPoint.position);
        line.SetPosition(1, weaponPoint.position + (Vector3)direction * 100f);

    }
}
