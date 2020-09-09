using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StartMenu : MonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUI pressAnyKey = null;
	private Color textColor;
	private Color alpha;
	private bool once = true;

	private void Awake()
	{
		textColor = pressAnyKey.color;
		alpha = new Color(textColor.r, textColor.g, textColor.b, 0.0f);
	}

	private void Update()
	{
		Color currentColor = Color.Lerp(alpha, textColor, Mathf.PingPong(Time.time, 1.5f));
		pressAnyKey.color = currentColor;

		if (Input.anyKeyDown && once)
		{
			once = false;
			AsyncSceneLoader.LoadAsyncAdditive("Base", this);
		}
	}
}
