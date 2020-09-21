using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flight : MonoBehaviour
{
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
	private static List<string> flightMember;
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
	public static int currentMemberIndex;

	private List<Character> flight;
	
	private void Awake()
	{
		flight = new List<Character>();
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
}
