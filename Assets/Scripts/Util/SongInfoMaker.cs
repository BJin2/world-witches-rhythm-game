using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class SongInfoMaker : MonoBehaviour
{
	private AudioSource source = null;
	private string path = "";
	private string infoname = "";
	[SerializeField]
	private SongInfo info;
	private int singer = 0;

	private void Awake()
	{
		source = GetComponent<AudioSource>();
		infoname = source.clip.name;
		path = Directory.GetCurrentDirectory() + @"\Assets\Resources\Audio\Song\" + infoname + @"\info.json";

		if (File.Exists(path))
		{
			SongLoader.LoadSongInfo(ref info, infoname);
		}
		else
		{
			info = new SongInfo
			{
				note = new List<Note>(),
				part = new List<Part>()
			};
		}
	}

	private void Update()
	{
		if (source.isPlaying)
		{
			//Divide part
			if (Input.GetKeyDown(KeyCode.Return))
			{
			
				singer = (singer + 1) % 5;
				info.part.Add(new Part(source.time, singer));
			
			}
			else if (Input.GetKeyDown(KeyCode.RightShift))//together
			{
				info.part.Add(new Part(source.time, -1));
			}


			//Note
			if (Input.GetKeyDown(KeyCode.A))
			{
				info.note.Add(new Note(source.time, 0, 0));
			}
			if (Input.GetKeyDown(KeyCode.S))
			{
				info.note.Add(new Note(source.time, 0, 1));
			}
			if (Input.GetKeyDown(KeyCode.D))
			{
				info.note.Add(new Note(source.time, 0, 2));
			}
			if (Input.GetKeyDown(KeyCode.F))
			{
				info.note.Add(new Note(source.time, 0, 3));
			}
			if (Input.GetKeyDown(KeyCode.G))
			{
				info.note.Add(new Note(source.time, 0, 4));
			}
		}
		else
		{
			if (Input.GetKeyDown(KeyCode.P))
			{
				source.Play();
			}
		}


		//End and save
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			
			
			if (File.Exists(path))
			{
				File.Delete(path);
			}

			// Sort info.note and info.part by their timing
			info.note.Sort((x, y) =>
			{
				return x.timing.CompareTo(y.timing);
			});
			info.part.Sort((x, y) =>
			{
				return x.timing.CompareTo(y.timing);
			});

			string jsonString = JsonUtility.ToJson(info);

			File.WriteAllText(path, jsonString);
		}

		//Remove note
		if (Input.GetKeyDown(KeyCode.N))
		{
			info.note.Clear();
		}
		//Remove part
		if (Input.GetKeyDown(KeyCode.M))
		{
			info.part.Clear();
		}
	}
}
