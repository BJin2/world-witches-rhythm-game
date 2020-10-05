#if UNITY_EDITOR
using UnityEngine;

public partial class Flight : MonoBehaviour
{
	private void OnValidate()
	{
		FindSpawnPositions();
	}

	private void OnDrawGizmos()
	{
		if (spawnPositions == null || spawnPositions.Count == 0)
			return;

		foreach (Vector3 spawnPosition in spawnPositions)
		{
			Gizmos.color = Color.green;
			Gizmos.DrawCube(spawnPosition, new Vector3(1, 1, 3));
		}
	}
}
#endif