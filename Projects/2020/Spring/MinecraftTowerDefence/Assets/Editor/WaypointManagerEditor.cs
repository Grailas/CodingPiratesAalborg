using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(WaypointSystem))]
public class WaypointSystemEditor : Editor
{
	WaypointSystem waypointSystem;

	private void OnEnable()
	{
		waypointSystem = (WaypointSystem)target;
	}


	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		GUILayout.Space(10f);
		GUILayout.Label("Tools", EditorStyles.boldLabel);

		GUILayout.BeginHorizontal();

		GUILayout.BeginVertical();
		if (GUILayout.Button(new GUIContent("Create \nWaypoint", "Creates and adds a new Waypoint to this WaypointSystem.")))
		{
			Undo.RecordObject(waypointSystem, "Created new waypoint");
			waypointSystem.CreateWaypoint();
			EditorUtility.SetDirty(waypointSystem);
		}
		GUILayout.EndVertical();

		GUILayout.BeginVertical();
		if (GUILayout.Button(new GUIContent("Cleanup \nlist", "Removes missing references from the WaypointSystem, and renames remaining Waypoints based on the set naming convention.")))
		{
			Undo.RecordObject(waypointSystem, "Waypoint system cleanup");
			waypointSystem.RefreshWaypoints();
			EditorUtility.SetDirty(waypointSystem);
		}
		GUILayout.EndVertical();

		GUILayout.EndHorizontal();

		if (waypointSystem.WaypointListContainsNull())
		{
			EditorGUILayout.HelpBox("There are missing or empty references in the list! Use 'Cleanup list' to quickly remove them.", MessageType.Warning);
		}

	}
}
