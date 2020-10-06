#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;

public partial class Flight : MonoBehaviour
{
	private void OnValidate()
	{
		FindSpawnPositions();
	}

	private void OnDrawGizmos()
	{
		if (Application.isPlaying)
			return;

		if (spawnPositions == null || spawnPositions.Count == 0)
			return;

		//Sorted by z position in camera space(Further to closer)
		List<Vector3> sortedPositions = new List<Vector3>();
		var sceneCamera = UnityEditor.SceneView.currentDrawingSceneView.camera;
		for(int i = 0; i < spawnPositions.Count; i++)
		{
			sortedPositions.Add(sceneCamera.worldToCameraMatrix.MultiplyPoint(spawnPositions[i]));
		}
		sortedPositions.Sort((x, y) => {
			float x_z = x.z;
			float y_z = y.z;
			return x_z.CompareTo(y_z);
		});

		foreach (Vector3 spawnPosition in sortedPositions)
		{
			//Make the position back to world space
			Vector3 worldPosition = sceneCamera.cameraToWorldMatrix.MultiplyPoint(spawnPosition);
			Gizmos.color = Color.green;
			Gizmos.DrawCube(worldPosition, new Vector3(1, 1, 3));
		}
	}
}
#endif