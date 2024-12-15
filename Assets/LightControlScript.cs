using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class LightControlScript : MonoBehaviour
{

    [SerializeField] private int timeSliced = 5;

    [SerializeField] private float areaRadius = 10;
    [SerializeField] private LayerMask defaultLayer;
    [SerializeField] private LayerMask targetLayer;

    private Collider2D[] colliders;

    int time = 0;


    private void Update()
    {
        if(time < timeSliced)
        {
            time++;
        }
        else
        {
            colliders = Physics2D.OverlapCircleAll(transform.position, areaRadius, targetLayer);

            if(colliders.Length > 0 )
                CastOnTargets(colliders);

            time = 0;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, areaRadius);
    }

    private void CastOnTargets(Collider2D[] colliders)
    {
        foreach(Collider2D collider in colliders)
        {
            Vector2 direction = collider.transform.position - transform.position;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, areaRadius, defaultLayer);

            if (hit.collider != null)
            {
                Debug.DrawLine(transform.position, hit.point);
                SpriteRenderer sprite = collider.gameObject.GetComponent<SpriteRenderer>();

                if (sprite == null || hit.collider.gameObject.name.StartsWith("Pocong"))
                    continue;

                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Enemies") || hit.collider.gameObject.layer == LayerMask.NameToLayer("Default") )
                {

                    sprite.enabled = true;
                }
                else
                {
                    sprite.enabled = false;
                }
            }
        }
    }










}
