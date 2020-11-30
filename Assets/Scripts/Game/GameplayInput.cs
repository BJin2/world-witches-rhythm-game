using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class GameplayInput
{
	public delegate void ProcessInput();

	private const int screenDiv = 5;

	private readonly int screenWidth = 0;
	private readonly KeyCode[] keys = null; //Keys for pressing lanes
	private readonly int[] keycodes = null; //All keycode values to find out pressed key

	public ProcessInput processGameplayInput = null;

	public GameplayInput(params KeyCode[] key)
	{
		screenWidth = Screen.width;

		keycodes = (int[])System.Enum.GetValues(typeof(KeyCode));

		keys = new KeyCode[screenDiv];
		for (int i = 0; i < screenDiv; i++)
		{
			if (i < key.Length)
				keys[i] = key[i];
			else
				keys[i] = KeyCode.None;
		}
	}

	public int ScreenDiv(Vector2 mousePosition)
	{
		return ScreenDiv(mousePosition.x);
	}
	public int ScreenDiv(Vector3 mousePosition)
	{
		return ScreenDiv(mousePosition.x);
	}
	public int ScreenDiv(float x)
	{
		int pos = Mathf.RoundToInt(x);
		return Mathf.Clamp((pos * screenDiv) / screenWidth, 0, 4);
	}
	public int ScreenDiv(KeyCode key)
	{
		for (int i = 0; i < screenDiv; i++)
		{
			if (keys[i] == key)
				return i;
		}

		return -1;
	}

	public bool AssignedKeyPresssed(KeyCode key)
	{
		for (int i = 0; i < keys.Length; i++)
		{
			if (keys[i] == key)
				return true;
		}
		return false;
	}

	public void DetermineInputType(ProcessInput touchVersion, ProcessInput keyboardVersion)
	{
		if (Input.touchSupported && (SystemInfo.deviceType == DeviceType.Handheld))
		{
			processGameplayInput = touchVersion;
		}
		else
		{
			processGameplayInput = keyboardVersion;
		}
	}

	public Queue<KeyCode> PressedKeys()
	{
		Queue<KeyCode> result = new Queue<KeyCode>();
		for (int i = 0; i < keycodes.Length; i++)
		{
			if (Input.GetKeyDown((KeyCode)keycodes[i]))
			{
				result.Enqueue((KeyCode)keycodes[i]);
			}
		}

		return result;
	}
}
