using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Flight : MonoBehaviour
{
#region used over different scenes
	public static readonly List<string> CHARACTER_NAME = new List<string>
	{
		"Miyafuji",
		"Sakamoto",
		"Lynette",
		"Perrine",
		"Minna",
		"Barkhorn",
		"Hartmann",
		"Charlotte",
		"Lucchini",
		"Sanya",
		"Eila"
	};
	//hard coded initializing for fast testing
	private static List<string> flightMember = new List<string>{
		"Miyafuji",
		"Sakamoto",
		"Lynette",
		"Perrine",
		"Minna"
	};
	public static List<string> FlightMember { 
		get 
		{
			if (flightMember == null)
			{
				Init();
			}
			return flightMember;
		} 
		set { flightMember = value; } }
	public static int currentMemberIndex = -1;
#endregion

	private List<Character> flight = null;
	private List<GameObject> member = null;

	[SerializeField]
	private List<Vector3> spawnPositions = null;
	
	private void Awake()
	{
		LoadMember();

		spawnPositions = new List<Vector3>();
		for (int i = 0; i < 5; i++)
		{
			spawnPositions.Add(transform.Find("CharacterPosition" + i.ToString()).position);
		}
	}

	private void Start()
	{
		SpawnMember();
	}

	public static void Init()
	{
		flightMember = new List<string>();
		flightMember.Clear();
		for (int i = 0; i < 5; i++)
		{
			flightMember.Add("");
		}
		currentMemberIndex = -1;
	}
	private void LoadMember()
	{
		member = new List<GameObject>();
		foreach (string name in flightMember)
		{
			member.Add(Resources.Load("Prefabs/Character/" + name, typeof(GameObject)) as GameObject);
		}
	}
	private void SpawnMember()
	{
		flight = new List<Character>();
		for (int i = 0; i < member.Count; i++)
		{
			flight.Add(
				Instantiate(member[i], 
				spawnPositions[i], 
				member[i].transform.rotation, 
				transform).GetComponent<Character>());
			SongPlayer.Instance.AddAudio(flight.Last().GetComponent<AudioSource>());
		}
	}
}
