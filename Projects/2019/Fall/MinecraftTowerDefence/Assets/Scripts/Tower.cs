using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class Tower : MonoBehaviour
{
    public float range;
    public Turret turret;
    public float shotsPerSecond;
    private float delayPerShot;
    private float shotTimer;
    public ProjectileStats projectileStats;
    public GameObject projectileSpawn;
    public Enemy target;
    public Vector2 aimLocation;
    public List<Enemy> enemies;
    public bool predictiveAiming;
    public int predicitonSteps = 10;
    [SerializeField]
    private float projectileMaximumTraversalTime;
    [SerializeField]
    private float projectilePredictionStepSize;

    public void AddEnemyToList(Enemy newEnemy)
    {
        enemies.Add(newEnemy);
    }

    public void RemoveEnemyFromList(Enemy oldEnemy)
    {
        enemies.Remove(oldEnemy);
    }



    // Start is called before the first frame update
    void Start()
    {
        projectileMaximumTraversalTime = range / projectileStats.speed;
        projectilePredictionStepSize = range / predicitonSteps;
    }

    void Awake()
    {
        delayPerShot = 1f / shotsPerSecond;
    }

    // Update is called once per frame
    void Update()
    {
        shotTimer += Time.deltaTime;
        Aim();
    }

    private void OnValidate()
    {
        turret.GetComponent<CircleCollider2D>().radius = range;
    }

    private void OnDrawGizmosSelected()
    {
        if (target != null)
        {
            Gizmos.color = new Color(1, 1, 1);
            Gizmos.DrawLine(transform.position, target.transform.position);
            if (predictiveAiming)
            {
                Gizmos.color = new Color(1, 1, 0);
                Gizmos.DrawLine(transform.position, aimLocation);
            }
        }
    }


    private void Target()
    {
        float[] distances = new float[enemies.Count];

        for (int i = 0; i < enemies.Count; i++)
        {
            float result = 0f;
            bool colaFound = false;
            for (int j = 0; j < WaypointSystem.Instance.waypoints.Count; j++)
            {
                if (!colaFound)
                {
                    colaFound = enemies[i].target == WaypointSystem.Instance.waypoints[j];

                    if (colaFound)
                    {
                        result = Vector2.Distance(enemies[i].transform.position, WaypointSystem.Instance.waypoints[j].transform.position);

                        if (j < WaypointSystem.Instance.waypoints.Count - 1)
                        {
                            result += Vector2.Distance(WaypointSystem.Instance.waypoints[j].transform.position, WaypointSystem.Instance.waypoints[j + 1].transform.position);
                        }
                    }
                }
                else if (colaFound)
                {
                    if (j < WaypointSystem.Instance.waypoints.Count - 1)
                    {
                        result += Vector2.Distance(WaypointSystem.Instance.waypoints[j].transform.position, WaypointSystem.Instance.waypoints[j + 1].transform.position);
                    }
                }
            }

            distances[i] = result;
        }

        float shortest = Mathf.Infinity;

        for (int i = 0; i < distances.Length; i++)
        {
            if (distances[i] < shortest)
            {
                shortest = distances[i];
                target = enemies[i];
            }
        }
    }

    public void Aim()
    {
        //First check how many enemies there are, in order to find right target
        if (enemies.Count == 1)
        {
            target = enemies[0];
        }
        else if (enemies.Count > 1)
        {
            Target();
        }
        else
        {
            target = null;
        }

        //Set position to aim at if there is a target
        if (target != null)
        {
            if (predictiveAiming)
            {
                aimLocation = PredictFutureLocation();
            }
            else
            {
                aimLocation = target.transform.position;
            }
        }

        //Aim towards the target
        turret.transform.LookAt(aimLocation, Vector3.right);


        if (shotTimer >= delayPerShot && target != null)
        {
            Shoot();
            shotTimer = 0;
        }
    }

    private Vector2 PredictFutureLocation()
    {
        Vector2 simulatedPosition = target.transform.position;
        float simulatedDistanceToPosition = 0f;

        float simulatedProjectileTraversal = 0f;

        float maximumEnemyTraversal = target.enemyStats.speed * projectileMaximumTraversalTime;
        float enemyTraversalStep = maximumEnemyTraversal / predicitonSteps;

        //If the enemy cannot reach a waypoint within the travel time of the projectile 
        if (target.distanceToWaypoint > maximumEnemyTraversal)
        {
            //Simulate linearly (simple case)
            for (int i = 0; i < predicitonSteps; i++)
            {
                //Simulate a step for the enemy
                simulatedPosition += (Vector2)target.direction * enemyTraversalStep;
                simulatedProjectileTraversal += projectilePredictionStepSize;
                simulatedDistanceToPosition = Vector2.Distance(simulatedPosition, transform.position);

                //if the enemy is within the projectiles range at this simulation step
                if (simulatedDistanceToPosition < simulatedProjectileTraversal)
                {
                    return simulatedPosition;
                }
            }

            return simulatedPosition;
        }
        else
        {
            //Take changes in direction into account (difficult case) 
            WaypointSystem wps = WaypointSystem.Instance;
            Vector2 enemyDirection = target.direction;
            int waypointIndex = target.waypointIndex;
            Waypoint enemyTarget = target.target;
            float distanceToWaypoint;

            for (int i = 0; i < predicitonSteps; i++)
            {
                //Get the distance from the simulated enemy position to the target waypoint
                distanceToWaypoint = Vector2.Distance(simulatedPosition, (Vector2)enemyTarget.transform.position);

                //If the distance to the target waypoint is greater than one step
                if (distanceToWaypoint > enemyTraversalStep)
                {
                    //Simulate a step for the enemy linearly
                    simulatedPosition += enemyDirection * enemyTraversalStep;
                    simulatedProjectileTraversal += projectilePredictionStepSize;
                    simulatedDistanceToPosition = Vector2.Distance(simulatedPosition, transform.position);
                }
                else
                {
                    //if there are more waypoints after this one
                    if (waypointIndex < wps.waypoints.Count - 1)
                    {
                        //Figure out how much further the enemy would step
                        float overstep = enemyTraversalStep - distanceToWaypoint;

                        //Set the simulated position as the waypoints
                        simulatedPosition = enemyTarget.transform.position;

                        //Get the next waypoint
                        waypointIndex++;
                        enemyTarget = wps.waypoints[waypointIndex];

                        //Get new directions from old waypoint location to new waypoint
                        Vector2 heading = (Vector2)enemyTarget.transform.position - simulatedPosition;
                        distanceToWaypoint = heading.magnitude;
                        enemyDirection = heading / distanceToWaypoint;

                        //Simulate the rest of the step
                        simulatedPosition += enemyDirection * overstep;
                        simulatedProjectileTraversal += projectilePredictionStepSize;
                        simulatedDistanceToPosition = Vector2.Distance(simulatedPosition, transform.position);
                    }
                    else
                    {
                        //Target the last waypoint!
                        return enemyTarget.transform.position;
                    }


                }

                //if the enemy is within the projectiles range at this simulation step
                if (simulatedDistanceToPosition < simulatedProjectileTraversal)
                {
                    return simulatedPosition;
                }
            }
            return target.target.transform.position;
        }
    }

    /// <summary>
    /// Fires a Projectile.
    /// </summary>
    public void Shoot()
    {
        //Projectile p = Instantiate(projectilePrefab, projectileSpawn.transform.position, turret.transform.rotation);
        //p.projectileStats = projectileStats;

        Projectile p = ProjectileManager.Instance.GetPooledProjectile();
        p.SetupProjectile(projectileStats, projectileSpawn.transform.position, projectileSpawn.transform.rotation, this);
    }
}
