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

    public void DamagePlayerBy(float damage)
    {
        PlayerHealth -= damage;
    }

    public float GetCurrentPlayerHealth()
    {
        return PlayerHealth;
    }

    public float GetPlayerMaxHealth()
    {
        return PlayerMaxHealth;
    }

}
