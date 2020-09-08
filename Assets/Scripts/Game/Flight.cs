using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flight : MonoBehaviour
{
	private List<Character> flight;

	private void Awake()
	{
		flight = new List<Character>();
	}
}
