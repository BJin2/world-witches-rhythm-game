using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hangar : KeyAction
{
	protected override void OnTriggerEnter(Collider col)
	{
		InstructionTrigger(col.GetComponent<BaseCharacter>(), true, new TriggeredAction(TurnOnFormation), KeyCode.E, "Form a flight");
	}

	protected override void OnTriggerExit(Collider col)
	{
		InstructionTrigger(col.GetComponent<BaseCharacter>(), false, new TriggeredAction(TurnOnFormation));
	}

	private void TurnOnFormation()
	{
		//disable camera movement and character movement
		Debug.Log("Formation UI ON");
	}
}
