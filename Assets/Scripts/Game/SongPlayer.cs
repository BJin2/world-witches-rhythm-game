using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SongPlayer : MonoBehaviour
{
	public static SongPlayer Instance { get; private set; }
	public static string songName = "Bookmark A Head";

	private int partIndex = 0;
	private SongInfo info = null;

	private AudioSource audioSource = null;
	private AudioClip[] song = null;

	[SerializeField]
	private float delayBeforeStart = 3.0f;
	private float delay = 0.0f;
	private float songLength = -1.0f;
	public float Timer { get; private set; }

	private void Awake()
	{
		Cursor.lockState = CursorLockMode.None;
		if (Instance == null || Instance != this)
			Instance = this;

		Timer = float.MinValue;
		audioSource = GetComponent<AudioSource>();
		SongLoader.Load(ref info, ref song, songName);
		songLength = song[0].length;
	}

	private void Start()
	{
		Time.timeScale = 0.0f;
		PauseDelay.Instance.AfterDelay += () => { Time.timeScale = 1.0f; };
		PauseDelay.Instance.Delay(delayBeforeStart);
		Timer = Spawner.Instance.GetOffset() * -1;
		Spawner.Instance.SpawnAll(info);
	}

	private void Update()
	{
		//Do nothing when song is over
		if (Timer > songLength)
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

		//For the one stated in this part(singer)
		//Play from the time in this part(timing)
		Part part = info.part[partIndex];
		audioSource.clip = song[part.singer];
		audioSource.time = part.timing;
		audioSource.Play();

		//Prepare for next;
		partIndex++;
		if(partIndex < info.part.Count)
			delay = info.part[partIndex].timing;
	}
	public void Pause()
	{
		audioSource.Pause();
		Time.timeScale = 0.0f;
		PauseDelay.Instance.AfterDelay += Resume;
	}
	public void UnPause()
	{
		//Prevent pausedelay to run several times over and over
		if (Time.timeScale != 0.0f)
			return;

		PauseDelay.Instance.Delay(3.0f);
	}
	private void Resume()
	{
		Time.timeScale = 1.0f;
		audioSource.UnPause();
	}
	public bool IsPaused()
	{
		return Time.timeScale == 0.0f;
	}
}
