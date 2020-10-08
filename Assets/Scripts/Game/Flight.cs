using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public partial class Flight : MonoBehaviour
{
#region used over different scenes (Character selection)
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
		"Charlotte",
		"Barkhorn",
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
#endregion

	private List<Character> flight = null;
	private List<GameObject> member = null;

	private List<Vector3> spawnPositions = null;
	public Neuroi[] closestNeurois { get; private set; }

	[SerializeField]
	private KeyCode[] keys = null; // keys for game play input
	
	GameplayInput gInput = null;
	
	private void Awake()
	{
		closestNeurois = new Neuroi[5];
		gInput = new GameplayInput(keys);
		gInput.DetermineInputType(TouchInput, KeyboardInput);

		LoadMember();

		if (spawnPositions == null)
			FindSpawnPositions();
	}

	private void Start()
	{
		SpawnMember();
	}

	private void Update()
	{
		if (SongPlayer.Instance.IsPaused())
			return;

		for (int i = 0; i < 5; i++)
		{
			if (IsMissing(closestNeurois[i]))
			{
				closestNeurois[i] = Spawner.Instance.GetFirstActiveNeuroiOnLane(i);
				flight[i].SetClosest(closestNeurois[i]);
			}
		}

		gInput.processGameplayInput?.Invoke();

		//Deal with missed neurois
		while (Spawner.Instance.CrashedNeurois.Count > 0)
		{
			Shield(Spawner.Instance.CrashedNeurois.Dequeue().Lane);
		}
	}

#region Spawning Characters
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
			flight[i].SetLane(i);
		}
	}
	private void FindSpawnPositions()
	{
		spawnPositions = new List<Vector3>();
		for (int i = 0; i < 5; i++)
		{
			spawnPositions.Add(transform.Find("CharacterPosition" + i.ToString()).position);
		}
	}
#endregion


#region Gameplay control
	private void TouchInput()
	{
		//For touch input
		for (int i = 0; i < Input.touchCount; i++)
		{
			Touch t = Input.GetTouch(i);
			if(t.phase == TouchPhase.Began)
				Shoot(gInput.ScreenDiv(t.position.x));
		}
	}

	private void KeyboardInput()
	{
		//For keyboard input
		if (Input.anyKeyDown)
		{
			var pressed = gInput.PressedKeys();
			while (pressed.Count > 0)
			{
				Shoot(gInput.ScreenDiv(pressed.Dequeue()));
			}
		}
	}

	public void Shoot(int memberIndex)
	{
		if (memberIndex < 0)
			return;

		flight[memberIndex].Shoot();

		if (!IsMissing(closestNeurois[memberIndex]))
			closestNeurois[memberIndex].ShootDown();
	}

	public void Shield(int memberIndex)
	{
		//Debug.Log("Shield");
		flight[memberIndex].Shield();
	}
	#endregion

	public static bool IsMissing(UnityEngine.MonoBehaviour obj)
	{
		if (obj == null)
			return true;

		try
		{
			obj.GetInstanceID();
			return !obj.gameObject.activeInHierarchy;
		}
		catch
		{
			return true;
		}
	}
}
