using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Waypoint : MonoBehaviour
{
	public WaypointSystem waypointSystem;

	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}

	void OnDrawGizmosSelected()
	{
		if (waypointSystem != null)
		{
			waypointSystem.DrawPath(this);
		}
		else
		{
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere(transform.position, 0.2f);
		}
	}
}
