using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public int EnemyLevelStarter = 100;
    public int EnemyLevelMinimum = 50;

    [SerializeField] private EnemyDatabase enemyDatabase;

    private int currEnemyLevel = 0;

    private List<string> enemyNames = new List<string>();

    private void Start()
    {
        //StartCoroutine(SpawnEnemies());
    }


    private IEnumerator SpawnAllEnemies()
    {
        if (currEnemyLevel >= EnemyLevelMinimum) yield break;

        while (currEnemyLevel < EnemyLevelMinimum)
        {
            RandomEnemySpawn();
        }

        yield return null;  
    }


    private void RandomEnemySpawn()
    {

        currEnemyLevel -= 15;
    }

}

public enum EnemyState
{
    Active, Idle, Dead
}