using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class KeyAction : MonoBehaviour
{
	[SerializeField]
	private GameObject instructionUI = null;
	[SerializeField]
	private TextMeshProUGUI keyText = null;
	[SerializeField]
	private TextMeshProUGUI actionText = null;

	public delegate void TriggeredAction();

	protected abstract void OnTriggerEnter(Collider col);
	protected abstract void OnTriggerExit(Collider col);

	protected void InstructionTrigger(BaseCharacter target, bool on, TriggeredAction triggeredAction, KeyCode key = KeyCode.None, string action = "")
	{
		if (!target)
			return;

		if (on)
		{
			instructionUI.SetActive(true);
			keyText.text = key.ToString();
			actionText.text = action;
			target.AssignAction(key, triggeredAction);
		}
		else
		{
			if (!target.ActionRemovable(triggeredAction))
				return;

			target.AssignAction(KeyCode.None, null);
			keyText.text = "";
			actionText.text = "";
			instructionUI.SetActive(false);
		}
	}

	protected void ChangeActionText(string toChange)
	{
		actionText.text = toChange;
	}
	protected void ChangeKeyText(string toChange)
	{
		keyText.text = toChange;
	}
}
