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
	private float timer = 0.0f;

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			Destroy(this);
		}

		audioSource = new List<AudioSource>();
		SongLoader.Load(ref info, ref song, songName);

		//TODO start playing after delay second
	}

	private void Update()
	{
		timer += Time.deltaTime;
		if (timer >= delay)
		{
			Play();
		}
	}

	public void Play()
	{
		//Read info.part and change the one playing audio
		//Access each Character with Flight.flight and info.part[?].part
		if (partIndex >= info.part.Count)
			return;

		if (partIndex > 0)
			audioSource[info.part[partIndex - 1].singer].Stop();

		//audioSource[info.part[partIndex].singer].PlayScheduled(info.part[partIndex].timing);
		audioSource[info.part[partIndex].singer].time = info.part[partIndex].timing;
		audioSource[info.part[partIndex].singer].Play();

		//Prepare for next;
		partIndex++;
		if(partIndex < info.part.Count)
			delay = info.part[partIndex].timing;
	}

	public void AddAudio(AudioSource aud)
	{
		audioSource.Add(aud);
	}

	public void AssignAudio()
	{
		for (int i = 0; i < audioSource.Count; i++)
		{
			audioSource[i].clip = song[i];
		}
	}
}
