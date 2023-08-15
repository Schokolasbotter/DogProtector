using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public enum SpawnState { SPAWNING, WAITING, COUNTING };
    [System.Serializable]
    public class Wave
    {
        public string name;
        public Transform Knight;
        public Transform Magician;

        public int countKnight;
        public int countMagician;

        public float rate;
    }

    public Wave[] waves;
    private int nextWave = 0;

    public Transform[] spawnPoints;
     
    public float timeBetweenWaves = 5f;
    private float waveCountdown;

    private float searchCountdown = 1f;

    private SpawnState state = SpawnState.COUNTING;

    private int waveMultiplier = 1;
    private int enemyHealth = 1;
    private float enemySpeed = 1;
    public int healthIncr;
    public float speedIncr;

    private void Start()
    {
        if (spawnPoints.Length == 0)
        {
            Debug.LogError("No spawn points referenced");
        }
        if (gameObject.name == "EasyWaveController")
        {
            waveMultiplier = 1;
            enemyHealth = 1;
            enemySpeed = 1;
        }
        else if (gameObject.name == "MediumWaveController")
        {
            waveMultiplier = 2;
            enemyHealth = 2;
            enemySpeed = 2;
        }
        else if (gameObject.name == "HardWaveController")
        {
            waveMultiplier = 3;
            enemyHealth = 3;
            enemySpeed = 3;
        }

        waveCountdown = timeBetweenWaves;
    }

    private void Update()
    {
        if(GameController.instance.gamePlaying)
        {
            if (state == SpawnState.WAITING)
            {
                if (!enemyIsAlive())
                {
                    waveCompleted();
                }
                else
                {
                    return;
                }
            }
            if (waveCountdown <= 0)
            {
                if (state != SpawnState.SPAWNING)
                {
                    //Start Spawning Wave
                    StartCoroutine(spawnWave(waves[nextWave]));
                }
            }
            else
            {
                waveCountdown -= Time.deltaTime;
            }
        }
        
    }

    private void waveCompleted()
    {
        Debug.Log("Wave Completed");
        state = SpawnState.COUNTING;
        waveCountdown = timeBetweenWaves;

        if(nextWave+1 > waves.Length - 1)
        {
            nextWave = 0;
            waveMultiplier++;
            enemyHealth += healthIncr;
            enemySpeed *= speedIncr;
            Debug.Log("All waves complete. Looping...");
        }
        else
        {
            nextWave++;
        }
    }
    bool enemyIsAlive()
    {
        searchCountdown -= Time.deltaTime;
        if (searchCountdown <= 0)
        {
            searchCountdown = 1f;
            if (GameObject.FindGameObjectWithTag("Knight") == null && GameObject.FindGameObjectWithTag("Magician") == null)
            {
                return false;
            }
        }
        return true;
    }
    IEnumerator spawnWave(Wave _wave)
    {
        Debug.Log("Spawning Wave " + _wave.name);
        state = SpawnState.SPAWNING;
        GameController.instance.IncrWave();
        
        for (int i = 0; i < _wave.countKnight * waveMultiplier; i++)
        {
            _wave.Knight.GetComponent<KnightController>().hitPoints = enemyHealth;
            _wave.Knight.GetComponent<KnightController>().movementSpeed = enemySpeed;
            spawnEnemy(_wave.Knight);
            yield return new WaitForSeconds(1f /_wave.rate);
        }
        for (int i = 0; i < _wave.countMagician * waveMultiplier; i++)
        {
            _wave.Magician.GetComponent<MagicianController>().hitPoints = enemyHealth;
            _wave.Magician.GetComponent<MagicianController>().movementSpeed = enemySpeed;
            spawnEnemy(_wave.Magician);
            yield return new WaitForSeconds(1f / _wave.rate);
        }
        state = SpawnState.WAITING;
        yield break;
    }

    private void spawnEnemy(Transform _enemy)
    {
        Debug.Log("Spawning Enemy " + _enemy.name);
        Transform _sp = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Instantiate(_enemy, _sp.position, _sp.rotation);        
    }
}
