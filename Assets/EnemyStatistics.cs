using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class EnemyStatistics : MonoBehaviour
{

    [SerializeField] private float enemyDamage;
    [SerializeField] private float enemyHealth;
    [SerializeField] private Animator animator;

    private float enemyMaxHealth;

    private PlayerStatsScript playerStatsScript;

    [SerializeField] private List<Collider2D> colliders = new List<Collider2D>();

    public EnemyState enemyState;

    private void Start()
    {
        enemyMaxHealth = enemyHealth;
        playerStatsScript = PlayerStatsScript.Instance;
        enemyState = EnemyState.Idle;
        DoColliders(true);
    }

    public void AttackPlayer()
    {
        
        animator.SetBool("Attack", true);
    }

    public void StopAtkPlayer()
    {
        animator.SetBool("Attack", false);
    }

    public float GetEnemyHealth()
    {
        return enemyHealth;
    }

    public float GetEnemyMaxHealth()
    {
        return enemyMaxHealth;
    }

    public void DamagePlayer()
    {
        playerStatsScript.DamagePlayerBy(enemyDamage);
    }

    public void DamageEnemy(float damage)
    {
        AgroEnemy();

        enemyHealth -= damage;
        if (enemyHealth <= 0)
            DeadEnemy();
    }

    private void DeadEnemy()
    {
        enemyState = EnemyState.Dead;
        animator.SetBool("Dead", true);

        DoColliders(false);
    }

    private void DoColliders(bool x)
    {
        foreach (Collider2D col in colliders)
        {
            col.enabled = x;
        }
    }

    public void AgroEnemy()
    {
        if (enemyState == EnemyState.Idle)
        {
            enemyState = EnemyState.Active;
            animator.SetBool("Active", true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            AgroEnemy();
        }
    }
}
