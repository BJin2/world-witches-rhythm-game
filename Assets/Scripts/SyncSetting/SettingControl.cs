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

	private float timer = 0.0f;
	private float delay = float.MaxValue;
	private float syncOffset = 0.0f;

	private AudioSource aud = null;

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

		aud = gameObject.GetComponent<AudioSource>();
	}

	private void Start()
	{
		ResetNeuroi();
	}

	private void Update()
	{
		if(!testingNeuroi.gameObject.activeInHierarchy)
			timer += Time.deltaTime;

		if (timer >= delay)
		{
			testingNeuroi.gameObject.SetActive(true);
			timer = 0.0f;
		}

		if (testingNeuroi.transform.position.z < range[1])
		{
			ResetNeuroi();
		}
		gInput.processGameplayInput();
	}
	private void ResetNeuroi()
	{
		timer = 0.0f;
		testingNeuroi.transform.position = new Vector3(0, 0, range[0]);
		testingNeuroi.gameObject.SetActive(false);
		CalculateDelay();
		aud.time = 0.0f;
		aud.Play();
	}
	private void CalculateDelay()
	{
		float offset = (range[0] - HitRange.Instance.transform.position.z) / Neuroi.Speed;
		delay = aud.clip.length - offset + syncOffset;
	}

	private void TouchInput()
	{
		//For touch input
		if (Input.touchCount > 0)
		{
			Touch t = Input.GetTouch(0);
			if (t.phase == TouchPhase.Began)
			{
				Shoot();
				return;
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
					Shoot();
					return;
				}
			}
		}
	}

	private void Shoot()
	{
		if (testingNeuroi.ShootDown())
		{
			ResetNeuroi();
		}
	}

	public void ChangeSync(float amt)
	{
		syncOffset += amt;
		Mathf.Clamp(syncOffset, -5, 5);
	}
	public void ChangeSpeed(float amt)
	{
		Neuroi.Speed += amt;
		Mathf.Clamp(Neuroi.Speed, 1, 100);
	}

	#region Flow Control
	public void Pause()
	{
		Time.timeScale = 0.0f;
		PauseDelay.Instance.DarkBG(true);
		PauseDelay.Instance.AfterDelay += () => { Time.timeScale = 1.0f; PauseDelay.Instance.DarkBG(false); };
		SelectionWindow.Instance.Show("Exit Setting?", "", new List<SelectionWindow.ButtonInfo>
		{
			new SelectionWindow.ButtonInfo("Save&Exit", ()=>{ Save(); Exit(); }),
			new SelectionWindow.ButtonInfo("Cancel", UnPause),
			new SelectionWindow.ButtonInfo("Exit w/o Save", Exit)
		});
	}
	private void UnPause()
	{
		//Prevent pausedelay to run several times over and over
		if (Time.timeScale != 0.0f)
			return;

		PauseDelay.Instance.Delay(3.0f);
	}
	private void Save()
	{
		//TODO save sync and neuroi speed
	}
	private void Exit()
	{
		AsyncSceneLoader.LoadAsyncAdditive("Base", this); 
		Time.timeScale = 1.0f;
	}
	#endregion
}
