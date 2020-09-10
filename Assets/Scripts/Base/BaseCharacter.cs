using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCharacter : MonoBehaviour
{
	[SerializeField]
	private float moveSpeed = 1.0f;
	private float rotateSpeed;

	private Vector3 direction;
	private CameraMovement followingCam;

	private void Awake()
	{
		followingCam = Camera.main.GetComponentInParent<CameraMovement>();
	}

	private void Update()
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

		direction = new Vector3(direction.x, 0.0f, direction.z);
		direction.Normalize();
		transform.position += direction * moveSpeed * Time.deltaTime;
	}
}
