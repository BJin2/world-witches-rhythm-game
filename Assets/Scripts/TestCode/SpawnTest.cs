using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTest : MonoBehaviour
{
	private GameObject neuroi;

	private void Awake()
	{
		for (int i = 0; i < 100000; i++)
		{
			neuroi = Resources.Load("Prefabs/Neuroi", typeof(GameObject)) as GameObject;
		}
	}

	private void Start()
	{
		StartCoroutine(Spawn());
	}

	private IEnumerator Spawn()
	{
		for (int i = 0; i < 100; i++)
		{
			for (int j = 0; j < 10; j++)
			{
				Vector3 spawnPos = new Vector3(j * 2.5f, 0, i * 3.0f);
				Instantiate(neuroi, spawnPos, neuroi.transform.rotation);
			}
			yield return null;
		}
	}
}
