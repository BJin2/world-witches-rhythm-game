using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hangar : KeyAction
{
	[SerializeField]
	private GameObject hangarUI = null;
	[SerializeField]
	private List<MonoBehaviour> listToDiable = null;


	protected override void OnTriggerEnter(Collider col)
	{
		InstructionTrigger(col.GetComponent<BaseCharacter>(), true, new TriggeredAction(TurnOnFormation), KeyCode.E, "Form a flight");
	}

	protected override void OnTriggerExit(Collider col)
	{
		InstructionTrigger(col.GetComponent<BaseCharacter>(), false, new TriggeredAction(TurnOnFormation));
	}

	//This function must be passed by delegate to target script
	private void TurnOnFormation()
	{
		UIEnable(true);
	}

	//This function will be called by UI button or keyboard shortcut
	public void TurnOffFormation()
	{
		UIEnable(true);
	}

	private void UIEnable(bool enable)
	{
		//Diable other scripts that processes input
		foreach (MonoBehaviour toDiable in listToDiable)
		{
			toDiable.enabled = enable;
		}

		//Turn on hangar UI
		hangarUI.SetActive(enable);
		InitializeFormation();
	}

	private void InitializeFormation()
	{
		//Clear flight member slots
	}
}
