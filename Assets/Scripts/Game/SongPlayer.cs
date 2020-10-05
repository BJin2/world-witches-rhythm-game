using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongPlayer : MonoBehaviour
{
	public static SongPlayer Instance { get; private set; }
	public static string songName = "Bookmark A Head";

	private int partIndex = 0;
	private SongInfo info = null;

	private List<AudioSource> audioSource = null;
	private AudioClip[] song = null;

	[SerializeField]
	private float delayBeforeStart = 3.0f;
	private float delay = 0.0f;
	public float Timer { get; private set; }

	private void Awake()
	{
		Cursor.lockState = CursorLockMode.None;
		if (Instance == null || Instance != this)
			Instance = this;

		Timer = float.MinValue;
		audioSource = new List<AudioSource>();
		SongLoader.Load(ref info, ref song, songName);
	}

	private void Start()
	{
		//Let other scripts run process after loading is done
		Timer = (Mathf.Max(delayBeforeStart, Spawner.Instance.GetOffset())) * -1;
		Spawner.Instance.SpawnAll(info);
	}

	private void Update()
	{
		//Do nothing when song is over
		if (Timer > audioSource[0].clip.length)
			return;

		Timer += Time.deltaTime;
		if (Timer >= delay)
		{
			Play();
		}
	}

	//Read info.part and change the one playing audio
	private void Play()
	{
		//Prevent index range error
		if (partIndex >= info.part.Count)
			return;

		if (partIndex > 0)// Stop the previous one
			audioSource[info.part[partIndex - 1].singer].Stop();

		//For the one stated in this part(singer)
		//Play from the time in this part(timing)
		Part part = info.part[partIndex];
		audioSource[part.singer].time = part.timing;
		audioSource[part.singer].Play();

		//Prepare for next;
		partIndex++;
		if(partIndex < info.part.Count)
			delay = info.part[partIndex].timing;
	}

	public void AddAudio(AudioSource aud)
	{
		audioSource.Add(aud);
		if (audioSource.Count == Flight.FlightMember.Count)
			AddSong();
	}

	public void AddSong()
	{
		for (int i = 0; i < audioSource.Count; i++)
		{
			audioSource[i].clip = song[i];
		}
	}
}
