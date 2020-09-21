using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AudioTest : MonoBehaviour
{
	private List<AudioSource> source;

	private void Awake()
	{
		source = FindObjectsOfType<AudioSource>().ToList();
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			ToTime(17.5f);
		}
		else if (Input.GetKeyDown(KeyCode.R))
		{
			ToTime(0.0f);
		}
		else if (Input.GetKeyDown(KeyCode.Return))
		{
			ToTime(120.8f);
		}
	}

	private void ToTime(float time)
	{
		foreach (AudioSource aud in source)
		{
			aud.time = time;
			aud.Play();
		}
	}
}
