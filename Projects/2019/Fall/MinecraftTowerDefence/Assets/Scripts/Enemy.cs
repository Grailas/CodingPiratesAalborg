using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	[Header("Enemy stats")]
	public int id = 0;
	public EnemyStats enemyStats;
	public float health;
	public Healthbar healthBar;
	public SpriteRenderer spriteRenderer;


	[Header("Enemy directions")]
	public Waypoint target;
	public int waypointIndex;
	public Vector3 heading;
	public Vector3 direction;
	public float distanceToWaypoint;

	private Rigidbody2D rigidbody2D;

	// Start is called before the first frame update
	void Start()
	{

	}

	void Awake()
	{
		rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
	}

	// Update is called once per frame
	void Update()
	{

	}

	void FixedUpdate()
	{
		UpdateDirections();
		Move();
	}

	private void UpdateDirections()
	{
		if (target == null)
		{
			target = WaypointSystem.Instance.waypoints[0];
		}

		heading = target.transform.position - transform.position;
		distanceToWaypoint = heading.magnitude;
		direction = heading / distanceToWaypoint;
	}

	private void Move()
	{
		float moveDistance = enemyStats.GetFixedUpdatePositionChange();

		if (distanceToWaypoint > moveDistance) //If the distance to next waypoint is larger than how far this enemy will get
		{   //Move towards it normally
			rigidbody2D.MovePosition(transform.position + direction * moveDistance);
		}
		else if (waypointIndex < WaypointSystem.Instance.waypoints.Count - 1) //Else if there are more waypoints after this one
		{
			Vector3 tempPos = target.transform.position; //Store the location of the current waypoint

			moveDistance -= distanceToWaypoint; //Calculate the missing amount of movement
			waypointIndex++;
			target = WaypointSystem.Instance.waypoints[waypointIndex]; //Get the next waypoint

			heading = target.transform.position - tempPos;  //Get new directions from old waypoint location to new waypoint
			distanceToWaypoint = heading.magnitude;
			direction = heading / distanceToWaypoint;

			rigidbody2D.MovePosition(transform.position + direction * moveDistance); //Move the remaining bit towards new waypoint
		}
		else //Else simply move this enemy to the last waypoint
		{
			//TODO: Enemy has reached home!
			target = WaypointSystem.Instance.waypoints[WaypointSystem.Instance.waypoints.Count - 1];
			rigidbody2D.MovePosition(target.transform.position);
		}
	}

	public void TakeDamage(float damage)
	{
		health -= damage;
		if (health <= 0)
		{
			Die();
		}
		else
		{
			healthBar.SetCurrenthealthFill(health / enemyStats.maxHealth);
		}
	}

	private void Die()
	{
		gameObject.SetActive(false);
	}

	/// <summary>
	/// Resets the Enemy with new values.
	/// </summary>
	/// <param name="newStats">Stats for the Enemy.</param>
	/// <param name="newPosition">Starting position.</param>
	public void SetupEnemy(EnemyStats newStats, Vector3 newPosition)
	{
		enemyStats = newStats;
		spriteRenderer.sprite = enemyStats.appearance;
		health = enemyStats.maxHealth;
		healthBar.SetCurrenthealthFill(1f);
		transform.position = newPosition;
		target = null;
		waypointIndex = 0;
		gameObject.name = string.Format("Enemy {0} ({1})", id, enemyStats.name);

		gameObject.SetActive(true);
		UpdateDirections();
	}
}
