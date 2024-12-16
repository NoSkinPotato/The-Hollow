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

    private AudioManager audioManager;

    private void Start()
    {
        audioManager = AudioManager.Instance;
    }

    public void DamagePlayerBy(float damage)
    {
        if (Invulnerable) return;

        audioManager.Play("Hurt");

        PlayerHealth -= damage;
        if (PlayerHealth < 0)
        {
            Invulnerable = true;
            PlayerHealth = 0;
            GameManager.EndGame();
            gameObject.SetActive(false);
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
