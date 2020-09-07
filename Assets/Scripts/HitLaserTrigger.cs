using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitLaserTrigger : MonoBehaviour
{
	private void OnTriggerEnter(Collider col)
	{
		//make the colliding neuroi shoot laser
	}

	private void OnTriggerExit(Collider col)
	{
		//start decreasing the neuroi's score
	}
}
