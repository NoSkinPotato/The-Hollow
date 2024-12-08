using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSignal : MonoBehaviour
{

    public SoundLevel currLevel;

    [SerializeField] private CircleCollider2D col;

    private void Update()
    {
        switch (currLevel)
        {
            case SoundLevel.Silent:
                col.radius = 4;
                break;
            case SoundLevel.Medium:
                col.radius = 12;
                break;
            case SoundLevel.Loud:
                col.radius = 20;
                break;

        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.CompareTag("EnemyDetector"))
        {
            //ActivateEnemy
            EnemyStatistics stats = collision.gameObject.GetComponent<EnemyStatistics>();
            if (stats != null) {

                stats.AgroEnemy();
            }
            else
            {
                Debug.Log("Stats Signal Not Available");
            }
        }
    }
}

public enum SoundLevel
{
    Silent, Medium, Loud
}
