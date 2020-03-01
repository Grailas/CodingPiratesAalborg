using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{

	private static ProjectileManager _instance;
	public static ProjectileManager Instance
	{
		get
		{
			return _instance;
		}
	}

	[Header("Pool Settings")]
	public Projectile projectilePrefab;
	public List<Projectile> pooledProjectiles;
	public int startAmountToPool;

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

	// Update is called once per frame
	void Update()
	{

	}

	/// <summary>
	/// Creates a pool of Projectiles
	/// </summary>
	void SetupPool()
	{
		pooledProjectiles = new List<Projectile>();
		for (int i = 0; i < startAmountToPool; i++)
		{
			NewProjectile();
		}
	}

	/// <summary>
	/// Tries to find and return an inactive Projectile from the pool. 
	/// If no inactive Projectile is found, a new one is instantiated, added to list and returned.
	/// </summary>
	/// <returns>Projectile</returns>
	public Projectile GetPooledProjectile()
	{
		for (int i = 0; i < pooledProjectiles.Count; i++)
		{
			if (!pooledProjectiles[i].gameObject.activeInHierarchy)
			{
				return pooledProjectiles[i];
			}
		}

		return NewProjectile();
	}

	/// <summary>
	/// Instantiates and returns a new Projectile. Also automatically adds it to the pool.
	/// </summary>
	/// <returns>New Projectile</returns>
	private Projectile NewProjectile()
	{
		Projectile pro = Instantiate(projectilePrefab);
		pro.gameObject.SetActive(false);
		pooledProjectiles.Add(pro);
		pro.id = pooledProjectiles.Count;
		return pro;
	}
}
