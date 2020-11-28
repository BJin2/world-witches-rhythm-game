using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingControl : MonoBehaviour
{
	[SerializeField]
	private Neuroi testingNeuroi = null;
	[SerializeField]
	private KeyCode key = KeyCode.None; // keys for game play input
	GameplayInput gInput = null;

	private float[] range;

	private void Awake()
	{
		Neuroi.Speed = 20.0f;
		Neuroi.FindHitPosition();
		Neuroi.ClonePiece = true;
		testingNeuroi.SetLane(0);
		gInput = new GameplayInput(key);
		gInput.DetermineInputType(TouchInput, KeyboardInput);

		range = new float[2];
		for (int i = 0; i < 2; i++)
		{
			range[i] = transform.Find($"Range{i}").position.z;
		}
	}

	private void Update()
	{
		if (testingNeuroi.transform.position.z < range[1])
		{
			testingNeuroi.transform.position = new Vector3(0, 0, range[0]);
		}
		gInput.processGameplayInput();
	}

	private void TouchInput()
	{
		//For touch input
		if (Input.touchCount > 0)
		{
			Touch t = Input.GetTouch(0);
			if (t.phase == TouchPhase.Began)
			{
				testingNeuroi.ShootDown();
			}
		}
	}

	private void KeyboardInput()
	{
		//For keyboard input
		if (Input.anyKeyDown)
		{
			var pressed = gInput.PressedKeys();
			while (pressed.Count > 0)
			{
				if (gInput.AssignedKeyPresssed(pressed.Dequeue()))
				{
					if (testingNeuroi.ShootDown())
					{
						testingNeuroi.gameObject.SetActive(true);
						testingNeuroi.transform.position = new Vector3(0, 0, range[0]);
					}
					return;
				}
			}
		}
	}
}
