using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
	public static Spawner Instance { get; private set; }

	private List<Vector3> spawnPositions;

	private void Awake()
	{
		if (Instance == null)
			Instance = this;
		else
			Destroy(this);

		spawnPositions = new List<Vector3>();

		for (int i = 0; i < 5; i++)
		{
			spawnPositions.Add(transform.Find("SpawnPosition" + i.ToString()).position);
		}

		//Load neuroi modeling, speed settings and etc
	}

	public void Spawn()
	{
		//Speed setting && rhythm data
		//receive spawn data (Calculated spawn timing, position, type and initial speed)
	}
}
