using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class NeuroiSpawner : MonoBehaviour
{
	private List<Vector3> spawnPositions = null;
	private List<GameObject> neuroi_original = null;
	public event EventHandler SpawnDone = null;

	public void PrepareSpawn(ref float offset)
	{
		//TODO Add more types of neuroi
		neuroi_original = new List<GameObject>
		{
			Resources.Load("Prefabs/Neuroi", typeof(GameObject)) as GameObject
		};

		spawnPositions = new List<Vector3>();
		for (int i = 0; i < 5; i++)
		{
			spawnPositions.Add(transform.Find("SpawnPosition" + i.ToString()).position);
		}
		float dist = Mathf.Abs(spawnPositions[0].z - Neuroi.FindHitPosition());
		offset = dist / Neuroi.Speed;
	}

	public void SpawnAll(List<Note> spawnInfo, List<Neuroi> neurois)
	{
		StartCoroutine(SpawnChunk(spawnInfo.Count, 10, spawnInfo, neurois));
	}

	private IEnumerator SpawnChunk(int total, int chunk, List<Note> spawnInfo, List<Neuroi> neurois)
	{
		for (int i = 0; i < total; i++)
		{
			if ((i % chunk) == 0)
				yield return null;

			neurois.Add(
				Instantiate(
					neuroi_original[spawnInfo[i].type],
					spawnPositions[spawnInfo[i].position],
					neuroi_original[spawnInfo[i].type].transform.rotation,
					transform).GetComponent<Neuroi>());

			neurois.Last().SetLane(spawnInfo[i].position);
			neurois.Last().gameObject.SetActive(false);
		}

		SpawnDone?.Invoke(this, new EventArgs());
	}
}
