using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public partial class NeuroiManager : MonoBehaviour
{
	public static NeuroiManager Instance { get; private set; }

	private NeuroiSpawner spawner = null;
	private List<Note> spawnInfo = null;
	private List<Neuroi> neurois = null;
	public Queue<Neuroi> CrashedNeurois { get; private set; }// Missed neurois

	public int TotalNeuroi { get; private set; }

	private int spawnIndex = 0;
	private float delay = float.MaxValue;
	private float offset = float.MinValue;
	public float Offset { get { return offset; } }

	private void Awake()
	{
		if (Instance == null || Instance != this)
			Instance = this;

		//TODO move this to speed setting(when implemented)
		Neuroi.Speed = 20.0f;
		Neuroi.ClonePiece = false;

		neurois = new List<Neuroi>();
		CrashedNeurois = new Queue<Neuroi>();

		spawner = gameObject.AddComponent<NeuroiSpawner>();
		spawner.PrepareSpawn(ref offset);
		spawnIndex = 0;

		SongLoader.LoadingDone += SpawnAfterLoading;
		spawner.SpawnDone += ClearAfterSpawn;
	}

	private void Update()
	{
		if (SongPlayer.Instance.Timer >= delay)
		{
			ActivateNeuroi();
		}
	}
	private void SpawnAfterLoading(object sender, SongLoadEventArgs e)
	{
		spawnInfo = e.Info.note;
		spawner.SpawnAll(spawnInfo, neurois);
	}

	private void ClearAfterSpawn(object sender, System.EventArgs e)
	{
		delay = spawnInfo[0].timing - offset;
		TotalNeuroi = neurois.Count;
		Destroy(spawner);
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
}
