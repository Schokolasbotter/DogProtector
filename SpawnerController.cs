using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerController : MonoBehaviour
{
    public GameObject KnightPrefab;
    public GameObject MagicianPrefab;
    private GameObject enemyPrefab;
    public int enemyNumber;
    public int timeBetweenSpawn;
    private bool canSpawn;
    private int randomizer;

    private void Start()
    {
        canSpawn = true;
        SpawnWave();
    }

    public void SpawnWave ()
    {
        for (int i = 0; i < enemyNumber; i++)
        {
            if (canSpawn == true)
            {
                randomizer = Random.Range(0, 101);
                if (randomizer % 2 == 0)
                {
                    enemyPrefab = KnightPrefab;
                }
                else
                {
                    enemyPrefab = MagicianPrefab;
                }
                spawnEnemy();
            }
        }
    }
    private void spawnEnemy()
    {
        canSpawn = false;    
        Instantiate(enemyPrefab, transform.position, transform.rotation);        
        StartCoroutine(SpawnCooldown());
    }

    IEnumerator SpawnCooldown()
    {
        yield return new WaitForSeconds(timeBetweenSpawn);
        canSpawn = true;
    }
}
