using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [HideInInspector]
    public Vector2 playerPosition;

    public float movementSpeed;
    [SerializeField] private float rayCastDistance = 150f;
    [SerializeField] private LayerMask defaultLayer;
    public Rigidbody2D rb;
    [SerializeField] private EnemyPathfinder enemyPathfinder;

    private Vector2 direction;

    [SerializeField] private float AttackDistance = 2f;
    [SerializeField] private EnemyStatistics stats;
    [SerializeField] private LayerMask playerLayer;

    [SerializeField] private float rangeBuffer = 0.2f;
    [SerializeField] private float lookOffset = 90f;
    

    private void Start()
    {
        playerPosition = PlayerAnimationControl.Instance.transform.position;
    }

    private void FixedUpdate()
    {
        if (stats.enemyState != EnemyState.Active)
            return;


        playerPosition = PlayerAnimationControl.Instance.transform.position;
        direction = (playerPosition - (Vector2)transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, rayCastDistance, defaultLayer);

        if (hit.collider == null)
            return;


        if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Walls"))
        {
            enemyPathfinder.StartPF();
        }
        else
        {
            GoToPlayer();
        }


        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        rb.transform.rotation = Quaternion.Euler(0f, 0f, angle + lookOffset);
    }
    
    private void GoToPlayer()
    {
        if (enemyPathfinder != null) enemyPathfinder.StopPF();



        if (Vector2.Distance(transform.position, playerPosition) <= AttackDistance + rangeBuffer &&
            Vector2.Distance(transform.position, playerPosition) >= AttackDistance - rangeBuffer)
        {
            stats.AttackPlayer();
            return;
        }

        stats.StopAtkPlayer();
        rb.transform.position += (Vector3)direction * movementSpeed * Time.fixedDeltaTime;
    }

    private void Damageplayer()
    {
        stats.DamagePlayer();
    }

}
