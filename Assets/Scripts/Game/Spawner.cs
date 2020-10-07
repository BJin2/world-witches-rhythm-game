using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//TODO sync adjustment
public partial class Spawner : MonoBehaviour
{
	public static Spawner Instance { get; private set; }

	private List<Vector3> spawnPositions = null;
	private List<GameObject> neuroi_original = null;
	private List<Note> spawnInfo = null;

	private List<Neuroi> neurois = null;
	public Queue<Neuroi> CrashedNeurois { get; private set; }// Missed neurois

	private int spawnIndex = 0;
	private float delay = float.MaxValue;

	private float offset = float.MinValue;

	private void Awake()
	{
		if (Instance == null || Instance != this)
			Instance = this;

		//TODO move this to speed setting(when implemented)
		Neuroi.Speed = 20.0f;

		CrashedNeurois = new Queue<Neuroi>();

		LoadNeuroi();
	}

	private void Update()
	{
		if (SongPlayer.Instance.Timer >= delay)
		{
			ActivateNeuroi();
		}
	}

#region Spawning
	private void LoadNeuroi()
	{
		//TODO Add more types of neuroi
		neuroi_original = new List<GameObject>
		{
			Resources.Load("Prefabs/Neuroi", typeof(GameObject)) as GameObject
		};
	}

	private void FindSpawnPositions()
	{
		spawnPositions = new List<Vector3>();
		for (int i = 0; i < 5; i++)
		{
			spawnPositions.Add(transform.Find("SpawnPosition" + i.ToString()).position);
		}
	}

	public void SpawnAll(SongInfo info)
	{
		spawnInfo = info.note;
		StartCoroutine(Spawn(spawnInfo.Count, 10));
		//Speed setting && rhythm data
		//receive spawn data (Calculated spawn timing, position, type and initial speed)

		delay = spawnInfo[0].timing - offset;
		spawnIndex = 0;
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
			neurois.Last().SetLane(spawnInfo[i].position);
			neurois.Last().gameObject.SetActive(false);
		}
	}

	private void ActivateNeuroi()
	{
		if (spawnIndex >= spawnInfo.Count)
			return;

		neurois[spawnIndex].gameObject.SetActive(true);
		spawnIndex++;
		if (spawnIndex < spawnInfo.Count)
			delay = spawnInfo[spawnIndex].timing - offset;
	}
#endregion


#region Managing Neurois

	public void NeuroiCrashed(Neuroi neuroi)
	{
		CrashedNeurois.Enqueue(neuroi);
	}

	public Neuroi GetFirstActiveNeuroiOnLane(int lane)
	{
		var result = from n in neurois 
					 where n.gameObject.activeInHierarchy && (n.Lane == lane) 
					 orderby n.transform.position.z 
					 select n;

		var list = result.ToList();
		if (list != null && list.Count > 0)
			return list[0];


		return null;
	}

#endregion

//Just util function
	public float GetOffset()
	{
		if (spawnPositions == null)
			FindSpawnPositions();

		float dist = Mathf.Abs(spawnPositions[0].z - Neuroi.FindHitPosition());
		offset = dist / Neuroi.Speed;
		return offset;
	}
}