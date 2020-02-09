using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New EnemyWave", menuName = "EnemyWave", order = 52)]

public class EnemyWave : ScriptableObject
{

	[System.Serializable]
	public struct EnemySpawn
	{
		public EnemyStats enemyStats;
		public float spawnDelay;
	}

	public EnemySpawn[] spawnlist;




	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
