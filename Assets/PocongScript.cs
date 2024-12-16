using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PocongScript : MonoBehaviour
{

    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Collider2D collide;
    [SerializeField] private EnemyStatistics stats;
    [SerializeField] private float damageRate;
    [SerializeField] private LayerMask mask;
    [SerializeField] private float timeSpliced;
    [SerializeField] private float maxDistanceFromPlayer = 5f;

    [SerializeField] private AudioSource staticSound;

    float timer = 0;
    float splice = 0;

    private GridData grid;
    private PlayerAnimationControl player;
    public float offset;

    bool teleport = true;

    [HideInInspector]
    public bool onLight = false;
    public bool damagePlayer = false;

    bool justLowHealth = false;

    private void Start()
    {
        grid = GridData.Instance;
        player = PlayerAnimationControl.Instance;
        spriteRenderer.enabled = false;
    }
    private void Update()
    {

        if (stats.enemyState != EnemyState.Active)
        {
            spriteRenderer.enabled = true;
            return;
        }
        


        LightControl();

        if (teleport == true)
        {
            teleport = false;
            TeleportIn(); 
        }

        if (Vector2.Distance(player.playerPosition.position, transform.position) > maxDistanceFromPlayer && teleport == false)
        {
            teleport = true;
        }

        if(spriteRenderer.enabled == true)
        {
            LookAt(player.transform.position);
        }


        if (damagePlayer == true)
        {
            HurtPlayer();
        }

    }

    private void ActivatePocong(bool x)
    {
        spriteRenderer.enabled = x;
        collide.enabled = x;
    }

    private void LightControl()
    {
        if (onLight == false)
        {
            if (staticSound.isPlaying == true)
            {
                staticSound.Stop();
            }
            return;
        }

        

        if (splice >= timeSpliced)
        {
            Vector2 direction = (player.playerPosition.position - transform.position).normalized;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 15f, mask);
            if (hit.collider != null && hit.collider.gameObject.CompareTag("Player"))
            {
                spriteRenderer.enabled = true;
                damagePlayer = true;
            }
            else
            {
                spriteRenderer.enabled = false;
                damagePlayer = false;
            }

            splice = 0;
        }
        else
        {
            splice++;
        }
    }

    private void HurtPlayer()
    {
        if (staticSound.isPlaying == false)
        {
            staticSound.Play();
        }

        if (timer < damageRate) {
            timer += Time.deltaTime;
        }
        else
        {
            timer = 0;
            stats.DamagePlayer();
        }
    }

    private void LookAt(Vector2 pos)
    {
        Vector2 direction = (pos - (Vector2)transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle + offset);
    }

    private void TeleportOut()
    {
        spriteRenderer.enabled = false;
        collide.enabled = false;
    }

    private void TeleportIn()
    {
        collide.enabled = true;
        spriteRenderer.enabled = false;

        Vector2 pos = grid.FindSpawnPointFromPlayer(8, 2);
        if(pos != null )
        {
            transform.position = pos;
        }
    }

    public void OnLight(bool x)
    {
        onLight = x;
        if(x == false)
        {
            spriteRenderer.enabled = false;
            damagePlayer = false;

        }
    }


}
