using UnityEngine;

public class SongLoader
{
	public static void Load(ref SongInfo info, ref AudioClip[] clip, string songName)
	{
		LoadSong(ref clip, songName);
		//LoadSongInfo(ref info, songName);
	}

	//Load song audio files based on flight members
	private static void LoadSong(ref AudioClip[] clip, string songName)
	{
		clip = new AudioClip[5];
		for (int i = 0; i < 5; i++)
		{
			clip[i] = Resources.Load("Audio/Song/" + songName + "/" + Flight.FlightMember[i], typeof(AudioClip)) as AudioClip;
		}
	}

	//Load song information data(notes, timing and etc)
	private static void LoadSongInfo(ref SongInfo info, string songName)
	{
		string json = (Resources.Load(@"Audio/" + songName + "/info", typeof(TextAsset)) as TextAsset).text;
		info = JsonUtility.FromJson<SongInfo>(json);
	}
}
