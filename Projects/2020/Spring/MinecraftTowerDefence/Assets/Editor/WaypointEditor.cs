using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Waypoint))]
public class WaypointEditor : Editor
{
	Waypoint waypoint;
	WaypointSystem waypointSystem;



	private void OnEnable()
	{
		waypoint = (Waypoint)target;
		waypointSystem = waypoint.waypointSystem;
	}


	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		GUILayout.Space(10f);
		GUILayout.Label("Tools", EditorStyles.boldLabel);

		GUILayout.BeginHorizontal();
		GUILayout.BeginVertical();
		if (GUILayout.Button(new GUIContent("Link to \nWaypoint System", "Connects this Waypoint unless already linked to existing WaypointSystem.")))
		{
			LinkToWaypointSystem();
		}
		GUILayout.EndVertical();

		GUILayout.BeginVertical();
		if (GUILayout.Button(new GUIContent("Force link to \nWaypoint System", "Adds and connects this Waypoint to the existing WaypointSystem, EVEN if already added.")))
		{
			LinkToWaypointSystem(true);
		}
		GUILayout.EndVertical();
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		GUILayout.BeginVertical();
		if (GUILayout.Button(new GUIContent("Unlink from \nWaypoint System", "Removes a connection between Waypoint and WaypointSystem")))
		{
			RemoveFromWaypointSystem(false);
		}
		GUILayout.EndVertical();

		GUILayout.BeginVertical();
		if (GUILayout.Button(new GUIContent("Unlink all from \nWaypoint System", "Removes ALL connections between Waypoint and WaypointSystem")))
		{
			RemoveFromWaypointSystem(true);
		}
		GUILayout.EndVertical();
		GUILayout.EndHorizontal();


		GUILayout.Space(10f);

		GUILayout.BeginHorizontal();
		GUILayout.BeginVertical();
		if (GUILayout.Button(new GUIContent("Move up in \nWaypoint System", "Moves this Waypoint up in the list.")))
		{
			MoveWaypointInList(-1); 
		}
		GUILayout.EndVertical();

		GUILayout.BeginVertical();
		if (GUILayout.Button(new GUIContent("Move down in \nWaypoint System", "Moves this Waypoint down in the list.")))
		{
			MoveWaypointInList(1);
		}
		GUILayout.EndVertical();
		GUILayout.EndHorizontal();

	}

	/// <summary>
	/// Tries to connect the Waypoint and the Waypoint system together
	/// </summary>
	/// <param name="force">If true, the Waypoint will still be added even if it already appears in the WaypointManagers list.</param>
	void LinkToWaypointSystem(bool force = false)
	{
		//If there's no reference to a WaypointSystem...
		if (waypointSystem == null)
		{
			Debug.Log("No WaypointSystem reference found. Searching...");

			//Look for GameControllers
			GameObject[] gos = GameObject.FindGameObjectsWithTag("GameController");
			if (gos.Length > 1)
			{
				Debug.LogWarning("More than one object with GameController tag exists. Using first found...");
			}
			else if (gos.Length == 1)
			{
				Debug.Log("Found a GameController!..");
			}
			else
			{
				Debug.LogError("No GameController found!");
				return;
			}

			//Get the WaypointSystem from the first GameController
			WaypointSystem foundWPS = gos[0].GetComponent<WaypointSystem>();
			if (foundWPS)
			{   //Try to link to found WaypointSystem
				Undo.RecordObject(foundWPS, "Linked Waypoint to WaypointSystem");
				foundWPS.AddWaypoint(waypoint, !force);
				EditorUtility.SetDirty(foundWPS);

				SceneView.RepaintAll();
			}
			else
			{
				Debug.LogError($"{foundWPS} does not have WaypointSystem!");
			}
		}
		else
		{//Try to link to referenced WaypointSystem
			Undo.RecordObject(waypointSystem, "Linked Waypoint to WaypointSystem");
			waypointSystem.AddWaypoint(waypoint, !force);
			EditorUtility.SetDirty(waypointSystem);

			SceneView.RepaintAll();
		}
	}

	/// <summary>
	/// Disconnects the Waypoint from the WaypointSystem.
	/// </summary>
	/// <param name="removeAll">If true, all references to this Waypoint in the WaypointSystem will be removed recursively.</param>
	void RemoveFromWaypointSystem(bool removeAll)
	{
		if (waypointSystem != null)
		{
			Undo.RecordObject(waypointSystem, "Removed Waypoint from WaypointSystem");
			waypointSystem.RemoveWaypoint(waypoint, removeAll);
			EditorUtility.SetDirty(waypointSystem);

			SceneView.RepaintAll();
		}
		else
		{
			Debug.LogWarning($"{waypoint} has no reference to a WaypointSystem.");
		}
	}

	/// <summary>
	/// Moves the Waypoints placement in the list, if linked to a WaypointSystem. If multiple references to this Waypoint exist, the first found instance will be moved.
	/// </summary>
	/// <param name="indexChange">How far the Waypoint should be moved in the list. Negative numbers moves it forward.</param>
	void MoveWaypointInList(int indexChange)
	{
		if (waypointSystem != null)
		{
			Undo.RecordObject(waypointSystem, $"Moved Waypoint steps in WaypointSystem");
			waypointSystem.MoveWaypoint(waypoint, indexChange);
			EditorUtility.SetDirty(waypointSystem);

			SceneView.RepaintAll();
		}
		else
		{
			Debug.LogWarning($"{waypoint} has no link to a WaypointSystem!");
		}
	}
}
