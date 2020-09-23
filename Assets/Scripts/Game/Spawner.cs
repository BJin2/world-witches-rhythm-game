using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEditor;
using UnityEngine;

public class Spawner : MonoBehaviour
{
	public static Spawner Instance { get; private set; }

	private List<Vector3> spawnPositions = null;
	private List<GameObject> neuroi_original = null;
	private List<Note> spawnInfo = null;
	private List<Neuroi> neurois = null;

	private int spawnIndex = 0;
	private float delay = float.MaxValue;

	private float offset = float.MinValue;

	private void Awake()
	{
		if (Instance == null)
			Instance = this;
		else
			Destroy(this);
		Neuroi.Speed = 3.0f;
		spawnPositions = new List<Vector3>();
		for (int i = 0; i < 5; i++)
		{
			spawnPositions.Add(transform.Find("SpawnPosition" + i.ToString()).position);
		}

		float dist = Mathf.Abs(spawnPositions[0].z - FindObjectOfType<HitLaserTrigger>().transform.position.z);
		offset = dist / Neuroi.Speed;

		LoadNeuroi();
	}

	//*/TODO activate neuroi on right timing
	private void Update()
	{
		if (SongPlayer.Instance.Timer >= delay)
		{
			ActivateNeuroi();
		}
	}
	//*/
	private void ActivateNeuroi()
	{
		if (spawnIndex >= spawnInfo.Count)
			return;

		neurois[spawnIndex].gameObject.SetActive(true);
		spawnIndex++;
		if(spawnIndex < spawnInfo.Count)
			delay = spawnInfo[spawnIndex].timing - offset;
	}

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

	public void StartActivating()
	{
		delay = spawnInfo[0].timing-offset;
		spawnIndex = 0;
	}
}