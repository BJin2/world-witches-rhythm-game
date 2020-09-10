using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCharacter : MonoBehaviour
{
	[SerializeField]
	private float moveSpeed = 1.0f;
	[SerializeField]
	private float rotateSpeed = 1.0f;

	private Vector3 direction = Vector3.zero;
	private CameraMovement followingCam = null;

	private KeyAction.TriggeredAction action = null;
	private KeyCode trigger = KeyCode.None;


	private void Awake()
	{
		followingCam = Camera.main.GetComponentInParent<CameraMovement>();
	}

	private void Update()
	{
		Move();
		TriggerAction();
	}

	private void Move()
	{
		direction = Vector3.zero;
		if (Input.GetKey(KeyCode.W))
		{
			direction = followingCam.lookDirection;
		}
		else if (Input.GetKey(KeyCode.S))
		{
			direction = followingCam.lookDirection * -1;
		}

		if (Input.GetKey(KeyCode.D))
		{
			direction += followingCam.rightDirection;
		}
		else if (Input.GetKey(KeyCode.A))
		{
			direction -= followingCam.rightDirection;
		}



		if (direction != Vector3.zero)
		{
			//Play animation
			direction = new Vector3(direction.x, transform.position.y, direction.z);
			direction.Normalize();
			Quaternion look = Quaternion.LookRotation(direction, Vector3.up);
			transform.rotation = Quaternion.Slerp(transform.rotation, look, rotateSpeed * Time.deltaTime);
			transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
		}
	}

	private void TriggerAction()
	{
		if (trigger == KeyCode.None)
			return;

		if (Input.GetKeyDown(trigger))
		{
			action();
		}
	}

	public void AssignAction(KeyCode triggerKey, KeyAction.TriggeredAction triggeredAction)
	{
		trigger = triggerKey;
		action = triggeredAction;
	}
	public bool ActionRemovable(KeyAction.TriggeredAction triggeredAction = null)
	{
		if (action == null || action.Target == triggeredAction.Target)
			return true;

		return false;
	}
}
