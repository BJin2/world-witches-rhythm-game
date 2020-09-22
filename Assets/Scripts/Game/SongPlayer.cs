using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongPlayer : MonoBehaviour
{
	public static SongPlayer Instance { get; private set; }
	public static string songName = "Bookmark A Head";
	private List<AudioSource> audioSource = null;
	private SongInfo info = null;
	private int partIndex = 0;
	private AudioClip[] song = null;
	[SerializeField]
	private float delay = 3.0f;
	[SerializeField]
	private float timer = 0.0f;

	private void Awake()
	{
		//TODO mouse state none for testing
		Cursor.lockState = CursorLockMode.None;
		if (Instance == null || Instance != this)
		{
			Instance = this;
		}

		audioSource = new List<AudioSource>();
		SongLoader.Load(ref info, ref song, songName);
	}

	private void Start()
	{
		//Let other scripts run process after loading is done
		Spawner.Instance.SpawnAll(info);
	}

	private void Update()
	{
		//Do nothing when song is over
		if (timer > audioSource[0].clip.length)
			return;

		timer += Time.deltaTime;
		if (timer >= delay)
		{
			Play();
		}
	}

	//Read info.part and change the one playing audio
	public void Play()
	{
		//Prevent index range error
		if (partIndex >= info.part.Count)
			return;

		if (partIndex > 0)// Stop the previous one
			audioSource[info.part[partIndex - 1].singer].Stop();
		else// start from 0 for the first one playing audio
			timer = 0.0f;

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
