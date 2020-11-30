#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public partial class NeuroiManager : MonoBehaviour
{
	private List<Vector3> positions;
	private void OnDrawGizmos()
	{
		string path = "Icons\\SpawnerIcon.png";

		if (positions == null || positions.Count == 0)
			return;

		foreach (Vector3 spawnPosition in positions)
		{
			Gizmos.DrawIcon(spawnPosition, path, false);
		}
	}

	private void OnValidate()
	{
		positions = new List<Vector3>();
		for (int i = 0; i < transform.childCount; i++)
		{
			positions.Add(transform.GetChild(i).position);
		}
	}
}

#endif