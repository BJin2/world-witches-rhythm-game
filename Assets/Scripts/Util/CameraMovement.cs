using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
	public enum ControlType
	{
		Mouse,
		Keyboard
	}

#region Inspector Variables
	[HideInInspector]
	public ControlType controlType = ControlType.Mouse;
	[HideInInspector]
	public float sensitivity = 5.0f;

	[HideInInspector]
	public bool invertY = false;

	[HideInInspector]
	public Transform target = null;

	[HideInInspector]
	public bool xRotation = true;
	[HideInInspector]
	public bool yRotation = true;

	[HideInInspector]
	public float xAxisMaxLimit = 50;
	[HideInInspector]
	public float xAxisMinLimit = -30;
	[HideInInspector]
	public float yAxisMaxLimit = 0;
	[HideInInspector]
	public float yAxisMinLimit = 0;

	[HideInInspector]
	public float invertYMultiplier = 1.0f;

	[HideInInspector]
	public LayerMask obstacleLayer;
	#endregion

	private float xDelta = 0;
	private float yDelta = 0;

	private Vector3 prevPosition;

	public Vector3 LookDirection { get { return transform.forward; } }
	public Vector3 RightDirection { get { return transform.right; } }

	private float followSpeed = 0.0f;

	private Transform cam;
	private Vector3 originalOffset;

	private void Awake()
	{
		prevPosition = Input.mousePosition;
		xDelta = 0;
		yDelta = 0;
		cam = GetComponentInChildren<Camera>().transform;
		originalOffset = cam.localPosition;
	}


	private void Update()
	{
		Rotate();
		Follow(transform, target);
		FlexibleDistance();
	}

	private void Follow(Transform moving, Transform to)
	{
		Follow(moving, to.position, 3.0f);
	}
	private void Follow(Transform moving, Vector3 to, float power)
	{
		float distance = Vector3.Distance(moving.position, to);
		Vector3 direction = (to - moving.position).normalized;
		if (distance > 0.1f)
		{
			followSpeed = Mathf.Pow(distance, power);
			moving.position += direction * followSpeed * Time.deltaTime;
		}
	}

	private void Rotate()
	{
		switch (controlType)
		{
			case ControlType.Mouse:
				Vector3 current = Input.mousePosition;
				xDelta = current.x - prevPosition.x;
				yDelta = current.y - prevPosition.y;
				prevPosition = current;
				break;
			case ControlType.Keyboard:

				if (Input.GetKey(KeyCode.UpArrow))
				{
					yDelta = 10.0f;
				}
				else if (Input.GetKey(KeyCode.DownArrow))
				{
					yDelta = -10.0f;
				}
				else
				{
					yDelta = 0.0f;
				}

				if (Input.GetKey(KeyCode.LeftArrow))
				{
					xDelta = -10.0f;
				}
				else if (Input.GetKey(KeyCode.RightArrow))
				{
					xDelta = 10.0f;
				}
				else
				{
					xDelta = 0.0f;
				}
				break;
		}

		if (xDelta != 0 || yDelta != 0)
		{
			float rotationX = transform.localEulerAngles.x - (yDelta * Time.deltaTime * sensitivity * invertYMultiplier);
			float rotationY = transform.localEulerAngles.y + (xDelta * Time.deltaTime * sensitivity);

			rotationX = AngleClamp(rotationX, xAxisMinLimit, xAxisMaxLimit);
			rotationY = AngleClamp(rotationY, yAxisMinLimit, yAxisMaxLimit);

			transform.localEulerAngles = new Vector3(rotationX, rotationY, 0.0f);
		}
	}

	private void FlexibleDistance()
	{
		Vector3 origin = target.position + new Vector3(0, 0.1f, 0);
		Vector3 dir = cam.position - origin;
		if (Physics.Raycast(origin, dir, out RaycastHit hit, originalOffset.magnitude*2, obstacleLayer))
		{
			Follow(cam, hit.point, 2.0f);
			Debug.DrawRay(origin, dir.normalized * hit.distance, Color.red);
		}
		else
		{
			Follow(cam, transform.TransformPoint(originalOffset), 1.5f);
			Debug.DrawRay(origin, dir.normalized * originalOffset.magnitude * 2, Color.green);
		}
	}

	private float AngleClamp(float value, float min, float max)
	{
		if (min == 0 && max == 0)
			return value;

		float minimun = 360 + min;

		if (value < 0.0f)
		{
			value = 360 + value;
		}

		if (value > max)
		{
			if (CloserToFirst(value, max, minimun))
			{
				return max;
			}
			else
			{
				if (value < minimun)
				{
					return minimun;
				}
			}
		}

		return value;
	}

	private bool CloserToFirst(float value, float num1, float num2)
	{
		if (Mathf.Abs(value - num1) <= Mathf.Abs(value - num2))
		{
			return true;
		}
		else
		{
			return false;
		}
	}
}
