using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointSystem : MonoBehaviour
{
	private static WaypointSystem _instance;
	public static WaypointSystem Instance
	{
		get
		{
			return _instance;
		}
	}

	/// <summary>
	/// Sets the naming convention for Waypoints. Use RefreshWaypoints to use.
	/// </summary>
	public string namingConvention = "WP";
	public Gradient waypointGradient;

	public List<Waypoint> waypoints;

	// Start is called before the first frame update
	void Start()
	{

	}

	private void Awake()
	{
		if (_instance != null && _instance != this)
		{
			Destroy(this);
		}
		else
		{
			_instance = this;
		}
	}

	// Update is called once per frame
	void Update()
	{

	}

	/// <summary>
	/// Creates and adds a new Waypoint to this WaypointSystem.
	/// </summary>
	public void CreateWaypoint()
	{
		GameObject newGO = new GameObject($"{namingConvention}{waypoints.Count + 1}");
		newGO.AddComponent<Waypoint>();
		newGO.tag = "Waypoint";
		AddWaypoint(newGO.GetComponent<Waypoint>());
	}

	/// <summary>
	/// Adds a Waypoint to the list.
	/// </summary>
	/// <param name="wp">Waypoint to add.</param>
	/// <param name="checkExisting">If true, only add if Waypoint doesn't exist in list.</param>
	public void AddWaypoint(Waypoint wp, bool checkExisting = false)
	{
		if (checkExisting && waypoints.Contains(wp))
		{
			Debug.Log($"{wp} already exists in list.");
			if (wp.waypointSystem != this)
			{
				wp.waypointSystem = this;
				Debug.Log($"... But Waypoint did not have correct connection to {this}. Relinked!");
			}
		}
		else
		{
			if (waypoints.Contains(null))
			{
				int i = waypoints.IndexOf(null);
				waypoints[i] = wp;
				wp.waypointSystem = this;
				Debug.Log($"Waypoint {wp} added to empty slot ({1})!");
			}
			else
			{
				waypoints.Add(wp);
				wp.waypointSystem = this;
				Debug.Log($"Waypoint {wp} added!");
			}
		}
	}

	/// <summary>
	/// Removes Waypoint from the list.
	/// </summary>
	/// <param name="wp">Waypoint to remove.</param>
	/// <param name="removeAll">If true, recursively remove all references to Waypoint.</param>
	public void RemoveWaypoint(Waypoint wp, bool removeAll)
	{
		if (removeAll)
		{
			if (waypoints.Contains(wp))
			{
				do
				{
					waypoints.Remove(wp);
				}
				while (waypoints.Contains(wp));

				wp.waypointSystem = null;
				Debug.Log($"All references between {wp} and {this} removed.");
			}
			else
			{
				Debug.LogWarning("No reference found!");
			}
		}
		else
		{
			if (waypoints.Contains(wp))
			{
				waypoints.Remove(wp);
				wp.waypointSystem = null;
				Debug.Log($"Reference between {wp} and {this} removed.");
			}
			else
			{
				Debug.LogWarning("No reference found!");
			}
		}
	}

	/// <summary>
	/// Moves a Waypoints placement in the list, if linked to this WaypointSystem. If multiple references to this Waypoint exist, the first found instance will be moved.
	/// </summary>
	/// <param name="indexChange">How far the Waypoint should be moved in the list. Negative numbers moves it forward.</param>
	public void MoveWaypoint(Waypoint wp, int indexChange)
	{
		int wpIndex = waypoints.IndexOf(wp);
		if (wpIndex > -1)
		{
			int targetIndex = Mathf.Clamp(wpIndex + indexChange, 0, waypoints.Count - 1);
			if (targetIndex != wpIndex)
			{
				Waypoint temp = waypoints[targetIndex];
				waypoints[targetIndex] = wp;
				waypoints[wpIndex] = temp;
				Debug.Log($"Waypoints {wpIndex} and {targetIndex} have swapped order.");
			}
		}
		else
		{
			Debug.LogWarning($"{wp} was not found in the list!");
		}
	}

	/// <summary>
	/// Removes missing references from the WaypointSystem, and renames remaining Waypoints based on the set naming convention.
	/// </summary>
	public void RefreshWaypoints()
	{
		//Go through the list backwards, and remove null references
		for (int i = waypoints.Count - 1; i > -1; i--)
		{
			if (waypoints[i] == null)
				waypoints.RemoveAt(i);
		}

		//Go through list forwards, and rename them according to their index
		for (int i = 0; i < waypoints.Count; i++)
		{
			waypoints[i].name = $"{namingConvention}{i + 1}";
		}
	}

	/// <summary>
	/// Checks if the list contaions null references.
	/// </summary>
	/// <returns>True if null is found, else false.</returns>
	public bool WaypointListContainsNull()
	{
		foreach (Waypoint wp in waypoints)
		{
			if (wp == null)
			{
				return true;
			}
		}
		return false;
	}

	/// <summary>
	/// Draws gizmo lines between the waypoints.
	/// </summary>
	/// <param name="wp">Waypoint to highlight</param>
	public void DrawPath(Waypoint wp)
	{
		//Find index in list to get colour
		int i = waypoints.IndexOf(wp);
		Gizmos.color = waypointGradient.Evaluate(i > 0 ? (1f / (waypoints.Count - 1)) * i : 0);
		Gizmos.DrawWireSphere(wp.transform.position, 0.2f);

		DrawPath();
	}

	/// <summary>
	/// Draws gizmo lines between the waypoints.
	/// </summary>
	public void DrawPath()
	{
		//If waypoint fields exist
		if (waypoints.Count > 0)
		{
			//If waypoint 0 exists, draw a gizmo for path start
			if (waypoints[0] != null)
			{
				Gizmos.color = waypointGradient.Evaluate(0);
				Gizmos.DrawWireCube(waypoints[0].transform.position, new Vector3(0.2f, 0.2f, 0.2f));
			}

			//If at least 2 waypoint fields exist
			if (waypoints.Count > 1)
			{
				Waypoint start = null, end = null;


				//Find a start for a line
				for (int i = 0; i < waypoints.Count - 1; i++)
				{
					if (waypoints[i] != null)
					{
						start = waypoints[i];
						Gizmos.color = waypointGradient.Evaluate((1f / (waypoints.Count - 1)) * i);

						//Find an end for a line
						for (int j = i + 1; j < waypoints.Count; j++)
						{
							if (waypoints[j] != null)
							{
								end = waypoints[j];
								//Debug.Log($"drawing line from {start} to {end}");
								Gizmos.DrawLine(start.transform.position, end.transform.position);
								break;
							}
							else
							{
								Debug.LogWarning($"Waypoint {j} not set!");
							}
						}
					}
					else
					{
						Debug.LogWarning($"Waypoint {i} not set!");
					}
				}

			}

			//If last waypoint exists, draw a gizmo for path end
			if (waypoints[waypoints.Count - 1] != null)
			{
				Gizmos.color = waypointGradient.Evaluate(1);
				Gizmos.DrawWireCube(waypoints[waypoints.Count - 1].transform.position, new Vector3(0.2f, 0.2f, 0.2f));
			}
		}
	}

	void OnDrawGizmos()
	{
		DrawPath();
	}
}
