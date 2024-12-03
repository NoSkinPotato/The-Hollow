using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatistics : MonoBehaviour
{

    [SerializeField] private float enemyDamage;
    [SerializeField] private float enemyHealth;
    [SerializeField] private Animator animator;

    private PlayerStatsScript playerStatsScript;

    public EnemyState enemyState;

    private void Start()
    {
        playerStatsScript = PlayerStatsScript.Instance;
        enemyState = EnemyState.Active;
    }


    public void AttackPlayer()
    {
        animator.SetBool("Attack", true);
    }

    public void StopAtkPlayer()
    {
        animator.SetBool("Attack", false);
    }

    public void DamagePlayer()
    {
        playerStatsScript.DamagePlayerBy(enemyDamage);
    }

    public void DamageEnemy(float damage)
    {
        enemyHealth -= damage;
        if (enemyHealth <= 0)
            DeadEnemy();

    }

    private void DeadEnemy()
    {
        enemyState = EnemyState.Dead;
        animator.SetBool("Dead", true);
    }

}
