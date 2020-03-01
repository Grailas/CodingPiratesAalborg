using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private static EnemyManager _instance;
    public static EnemyManager Instance
    {
        get
        {
            return _instance;
        }
    }

    private WaypointSystem waypointSystem;

    [Header("Pool Settings")]
    public Enemy enemyPrefab;
    public List<Enemy> pooledEnemies;
    public int startAmountToPool;

    [Header("Wave Settings")]
    public EnemyWave[] waves;
    private int wave = 0;
    public int GetWave()
    {
        return wave;
    }

    private bool firstWave = true;
    private int spawn = 0;
    public float gracePeriod = 5f;
    private bool gracePeriodActive = true;
    private float timer = 0f;


    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this);
        }
        else
        {
            _instance = this;
            SetupPool();
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        waypointSystem = gameObject.GetComponent<WaypointSystem>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        SpawnTimer();
    }

    /// <summary>
    /// Runs the spawn timer, which starts waves
    /// </summary>
    private void SpawnTimer()
    {
        //Run timer if there are more waves
        if (wave < waves.Length)
        {
            timer += Time.fixedDeltaTime;
        }

        //If the grace period is active
        if (gracePeriodActive)
        {
            //Check if the grace period time has passed
            if (timer > gracePeriod)
            {
                //Stop the grace period, allowing waves to spawn
                timer -= gracePeriod;
                gracePeriodActive = false;
                spawn = 0;
                if (!firstWave)
                {
                    wave++;
                }
                else
                {
                    firstWave = false;
                }
            }
        }

        //If the grace period is inactive, and there are remaining waves
        if (!gracePeriodActive && wave < waves.Length)
        {
            //Check if the delay for the spawn has passed
            if (timer >= waves[wave].spawnlist[spawn].spawnDelay)
            {
                //Spawn the enemy
                timer -= waves[wave].spawnlist[spawn].spawnDelay;
                EnemyStats enemyStats = waves[wave].spawnlist[spawn].enemyStats;
                Spawn(enemyStats);

                spawn++;

                //If this was the last enemy in the wave
                if (spawn >= waves[wave].spawnlist.Length)
                {
                    //Start the grace period
                    gracePeriodActive = true;
                }
            }
        }
    }

    /// <summary>
    /// Creates a pool of Enemies
    /// </summary>
    private void SetupPool()
    {
        pooledEnemies = new List<Enemy>();
        for (int i = 0; i < startAmountToPool; i++)
        {
            NewEnemy();
        }
    }

    /// <summary>
    /// Tries to find and return an inactive Enemy from the pool. 
    /// If no inactive Enemy is found, a new one is instantiated, added to list and returned.
    /// </summary>
    /// <returns>Enemy</returns>
    public Enemy GetPooledEnemy()
    {
        for (int i = 0; i < pooledEnemies.Count; i++)
        {
            if (!pooledEnemies[i].gameObject.activeInHierarchy)
            {
                return pooledEnemies[i];
            }
        }

        return NewEnemy();
    }

    /// <summary>
    /// Instantiates and returns a new Enemy. Also automatically adds it to the pool.
    /// </summary>
    /// <returns>New Enemy</returns>
    private Enemy NewEnemy()
    {
        Enemy enm = Instantiate(enemyPrefab);
        enm.gameObject.SetActive(false);
        pooledEnemies.Add(enm);
        enm.id = pooledEnemies.Count;
        return enm;
    }

    /// <summary>
    /// Spawns an enemy with the given stats
    /// </summary>
    /// <param name="stats">EnemyStats</param>
    private void Spawn(EnemyStats stats)
    {
        Enemy e = GetPooledEnemy();
        e.SetupEnemy(stats, waypointSystem.waypoints[0].transform.position);
    }
}
