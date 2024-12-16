using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
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
    public EnemyName enemyName;

    [SerializeField] private AudioSource AudioSource1;
    [SerializeField] private AudioSource AudioSource2;
    [SerializeField] private AudioSource AudioSource3;

    bool playSound = false;

    bool justActive = false;


    private void Start()
    {
        enemyMaxHealth = enemyHealth;
        playerStatsScript = PlayerStatsScript.Instance;
        enemyState = EnemyState.Idle;
        
        DoColliders(true);

    }


    private void Update()
    {
        if ((enemyName == EnemyName.Tuyul || enemyName == EnemyName.Genderuwo) && enemyState == EnemyState.Active && playSound == false)
        {
            StartCoroutine(TuyulSound());
        }


    }

    private IEnumerator TuyulSound()
    {
        if (AudioSource3 != null)
        {
            AudioSource3.Play();
        }
        

        float random = Random.Range(5, 15);
        playSound = true;
        yield return new WaitForSeconds(random);
        
        playSound = false;
        
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

        Debug.Log(gameObject.name + " Damaged: " +  damage);    

        enemyHealth -= damage;
        if (enemyHealth <= 0)
            DeadEnemy();
    }

    private void DeadEnemy()
    {
        if (AudioSource2 != null)
        {
            AudioSource2.Play();
            
        }
        if (AudioSource1 != null)
        {
            AudioSource1.Stop();

        }
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
