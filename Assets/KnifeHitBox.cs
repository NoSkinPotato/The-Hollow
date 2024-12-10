using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeHitBox : MonoBehaviour
{
    [SerializeField] private PlayerWeaponScript PlayerWeaponScript;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Knife");
            PlayerWeaponScript.DamageEnemy(collision, PlayerWeaponScript.knifeDamage);
        }
    }

}
