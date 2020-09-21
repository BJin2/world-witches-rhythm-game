using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

public class Spawner : MonoBehaviour
{
	public static Spawner Instance { get; private set; }

	private List<Vector3> spawnPositions = null;
	private List<Note> spawnInfo = null;
	private List<GameObject> neuroi_original = null;
	private List<Neuroi> neurois = null;

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

		LoadNeuroi();
	}

	/*/TODO activate neuroi on right timing
	private void Update()
	{
		
	}
	//*/
	private void LoadNeuroi()
	{
		//TODO Add more types of neuroi
		neuroi_original = new List<GameObject>
		{
			Resources.Load("Prefabs/Neuroi", typeof(GameObject)) as GameObject
		};
	}

	public void SpawnAll(SongInfo info)
	{
		spawnInfo = info.note;
		StartCoroutine(Spawn(spawnInfo.Count, 10));
		//Speed setting && rhythm data
		//receive spawn data (Calculated spawn timing, position, type and initial speed)
	}

	//Instantiate "chunk" amount of neurois per frame
	private IEnumerator Spawn(int total, int chunk)
	{
		neurois = new List<Neuroi>();
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
			neurois.Last().gameObject.SetActive(false);
		}
	}
}
