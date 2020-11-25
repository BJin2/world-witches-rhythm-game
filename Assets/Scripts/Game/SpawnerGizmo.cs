using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

#if UNITY_EDITOR
public partial class Spawner : MonoBehaviour
{
	private void OnDrawGizmos()
	{
		string path = "Icons\\SpawnerIcon.png";

		if (spawnPositions == null || spawnPositions.Count == 0)
			return;

		foreach (Vector3 spawnPosition in spawnPositions)
		{
			Gizmos.DrawIcon(spawnPosition, path, false);
		}
	}

	private void OnValidate()
	{
		FindSpawnPositions();
	}
}
#endif