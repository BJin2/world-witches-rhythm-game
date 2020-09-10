using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : KeyAction
{
	protected override void OnTriggerEnter(Collider col)
	{
		InstructionTrigger(col.GetComponent<BaseCharacter>(), true, new TriggeredAction(Open), KeyCode.E, "Open");
	}

	protected override void OnTriggerExit(Collider col)
	{
		InstructionTrigger(col.GetComponent<BaseCharacter>(), false, new TriggeredAction(Open));
	}

	public void Open()
	{
		Debug.Log("open");
	}
}
