using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
	public int id = 0;
	private Rigidbody2D rigidbody2D;
	public ProjectileStats projectileStats;
	private Tower origin;
	private bool hasHit = false;

	// Start is called before the first frame update
	void Start()
	{

	}

	void Awake()
	{
		rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
	}

	void FixedUpdate()
	{
		projectileStats.MoveProjectile(rigidbody2D);

		if (ProjectileOutOfRange())
		{
			gameObject.SetActive(false);
		}
	}

	/// <summary>
	/// Resets the projectile with new values.
	/// </summary>
	/// <param name="newStats">Stats for the Projectile.</param>
	/// <param name="newPosition">Starting position.</param>
	/// <param name="newRotation">Starting rotation.</param>
	/// <param name="newOrigin">What Tower the Projectile comes from.</param>
	public void SetupProjectile(ProjectileStats newStats, Vector3 newPosition, Quaternion newRotation, Tower newOrigin)
	{
		//origin = newOrigin;
		projectileStats = newStats;
		transform.position = newPosition;
		transform.rotation = newRotation;
		GetComponent<SpriteRenderer>().sprite = projectileStats.appearance;
		gameObject.name = string.Format("Projectile {0} ({1})", id, projectileStats.name);
		origin = newOrigin;
		hasHit = false;

		//TODO: Once there's new variables, remember to reset them here.

		gameObject.SetActive(true);
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		//Debug.Log("Collision!: " + this.name + " has hit " + collision.transform.name + ".");
		if (collision.transform.tag == "Enemy" && hasHit == false)
		{
			collision.gameObject.GetComponent<Enemy>().TakeDamage(projectileStats.damage);
		}
		hasHit = true;
		this.gameObject.SetActive(false);
	}

	private float GetDistanceFromTower()
	{
		float result;

		Vector3 temporary = transform.position - origin.transform.position;
		result = temporary.magnitude;

		return result;
	}

	public bool ProjectileOutOfRange()
	{
		bool result;

		result = GetDistanceFromTower() >= origin.range;

		return result;
	}
}
