using System.Collections.Generic;
using UnityEngine;

public class SongPlayer : MonoBehaviour
{
	public static SongPlayer Instance { get; private set; }
	public static string songName = "Bookmark A Head";
	private List<AudioSource> audioSource = null;
	private SongInfo info = null;
	private AudioClip[] song = null;
	[SerializeField]
	private float delay = 3.0f;

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

	public void Play()
	{
		//Read info.part and change the one playing audio
		//Access each Character with Flight.flight and info.part[?].part
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
