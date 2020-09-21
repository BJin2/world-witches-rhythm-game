using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongPlayer : MonoBehaviour
{
	public static SongPlayer Instance { get; private set; }

	//TODO temporary fixed song for now
	public static string songName = "Bookmark A Head";
	private AudioSource source = null;
	public SongInfo info;
	public AudioClip[] song;
	public float delay = 3.0f;

	private void Awake()
	{
		source = GetComponent<AudioSource>();
		SongLoader.Load(ref info, ref song, songName);
	}

	public void Play()
	{

	}
}
