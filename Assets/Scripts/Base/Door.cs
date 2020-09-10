using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : KeyAction
{
	private bool opened = false;
	[SerializeField]
	private List<Transform> hinges;
	[SerializeField]
	private float doorOpenAngle = 120.0f;
	[SerializeField]
	private float openSpeed = 1.0f;

	protected override void OnTriggerEnter(Collider col)
	{
		string action = opened ? "Close" : "Open";
		InstructionTrigger(col.GetComponent<BaseCharacter>(), true, new TriggeredAction(Open), KeyCode.E, action);
	}

	protected override void OnTriggerExit(Collider col)
	{
		InstructionTrigger(col.GetComponent<BaseCharacter>(), false, new TriggeredAction(Open));
	}

	public void Open()
	{
		StopAllCoroutines();

		if (opened)
		{
			foreach (Transform hinge in hinges)
			{
				StartCoroutine(AngleToward(hinge, 0.0f));
			}
			opened = false;
			ChangeActionText("Open");
		}
		else
		{
			float doorDirection = 1;
			foreach (Transform hinge in hinges)
			{
				StartCoroutine(AngleToward(hinge, doorOpenAngle * doorDirection));
				doorDirection *= -1;
			}
			opened = true;
			ChangeActionText("Close");
		}
	}

	private IEnumerator AngleToward(Transform target, float angle)
	{
		if (angle < 0)
			angle = 360 + angle;
		Quaternion targetRot = Quaternion.AngleAxis(angle, Vector3.up);
		//Rotate toward angle 0->angle or angle->0
		while (Mathf.Abs(target.localEulerAngles.y - angle) >= 0.1f)
		{
			//Debug.Log(target.name + " Opening " + target.localEulerAngles.y);
			target.localRotation = Quaternion.Lerp(target.localRotation, targetRot, openSpeed * Time.deltaTime);
			yield return null;
		}
	}
}
