using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitLaserTrigger : MonoBehaviour
{
	private void OnTriggerEnter(Collider col)
	{
		//make the colliding neuroi shoot laser
		Neuroi neuroi = col.GetComponent<Neuroi>();
		neuroi.Shoot();
	}

	private void OnTriggerExit(Collider col)
	{
		//start decreasing the neuroi's score
		Neuroi neuroi = col.GetComponent<Neuroi>();
		neuroi.Shoot();
	}
}
