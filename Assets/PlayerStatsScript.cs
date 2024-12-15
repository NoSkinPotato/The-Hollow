using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsScript : MonoBehaviour
{

    public static PlayerStatsScript Instance { get; private set; }


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    [SerializeField] private float PlayerHealth;
    [SerializeField] private float PlayerMaxHealth;
    [SerializeField] private bool Invulnerable = false;
    [SerializeField] private GameManager GameManager;

    public void DamagePlayerBy(float damage)
    {
        if (Invulnerable) return;

        PlayerHealth -= damage;
        if (PlayerHealth < 0)
        {
            PlayerHealth = 0;
            GameManager.EndGame();
        }
        
    }

    public float GetCurrentPlayerHealth()
    {
        return PlayerHealth;
    }

    public float GetPlayerMaxHealth()
    {
        return PlayerMaxHealth;
    }

    public void HealPlayer(float health)
    {
        float x = PlayerHealth + health;
        if(x >= PlayerMaxHealth)
        {
            PlayerHealth = PlayerMaxHealth;
        }
        else
        {
            PlayerHealth = x;
        }
    }

    public void SetPlayerInvulnerability(bool x)
    {
        Invulnerable = x;
    }

    public void SetPlayerHealth(float health)
    {
        PlayerHealth = health;
    }
}
