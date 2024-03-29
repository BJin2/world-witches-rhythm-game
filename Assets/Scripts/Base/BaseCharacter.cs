﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCharacter : MonoBehaviour
{
	[SerializeField]
	private float moveSpeed = 1.0f;
	[SerializeField]
	private float rotateSpeed = 1.0f;

	private Vector3 direction = Vector3.zero;
	[SerializeField]
	private CameraMovement followingCam = null;

	private KeyAction.TriggeredAction action = null;
	private KeyCode trigger = KeyCode.None;

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
			direction = followingCam.LookDirection;
		}
		else if (Input.GetKey(KeyCode.S))
		{
			direction = followingCam.LookDirection * -1;
		}

		if (Input.GetKey(KeyCode.D))
		{
			direction += followingCam.RightDirection;
		}
		else if (Input.GetKey(KeyCode.A))
		{
			direction -= followingCam.RightDirection;
		}



		if (direction != Vector3.zero)
		{
			//Play animation
			direction = new Vector3(direction.x, 0, direction.z);
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
			action?.Invoke();
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
