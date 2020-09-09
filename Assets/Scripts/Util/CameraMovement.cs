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

	[HideInInspector]
	public ControlType controlType = ControlType.Mouse;
	[HideInInspector]
	public float sensitivity = 5.0f;

	[HideInInspector]
	public bool invertY = false;

	[HideInInspector]
	public float follotSpeed = 5.0f;
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

	private float xDelta = 0;
	private float yDelta = 0;

	private Vector3 prevPosition;

	[HideInInspector]
	public float invertYMultiplier = 1.0f;

	private void Awake()
	{
		prevPosition = Input.mousePosition;
		xDelta = 0;
		yDelta = 0;
	}


	private void Update()
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
			
			transform.localEulerAngles = new Vector3(rotationX, rotationY, transform.localEulerAngles.z);
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
