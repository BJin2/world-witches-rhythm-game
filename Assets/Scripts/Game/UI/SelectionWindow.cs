using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SelectionWindow : MonoBehaviour
{
	public delegate void ButtonEventHandler();
	public static SelectionWindow Instance { get; private set; }

	public struct ButtonInfo
	{
		public string buttonText;
		public ButtonEventHandler eventHandler;
		public ButtonInfo(string text, ButtonEventHandler handler)
		{
			buttonText = text;
			eventHandler = handler;
		}
		public override string ToString()
		{
			string result = "ButtonInfo : \n";
			result += $"Button Text : {buttonText}\n";
			result += $"Event Handler : {eventHandler.ToString()}\n";
			return result;
		}
	}

	[SerializeField]
	private GameObject window;
	private TextMeshProUGUI title;
	private TextMeshProUGUI instruction;
	private Button[] buttons;

	private void Awake()
	{
		Instance = this;
		title = window.transform.Find("TitleBG").GetComponentInChildren<TextMeshProUGUI>();
		instruction = window.GetComponentInChildren<TextMeshProUGUI>();
		buttons = new Button[3];
		buttons[0] = window.transform.Find("Left").GetComponent<Button>();
		buttons[1] = window.transform.Find("Right").GetComponent<Button>();
		buttons[2] = window.transform.Find("Middle").GetComponent<Button>();

		foreach (Button b in buttons)
		{
			b.gameObject.SetActive(false);
		}
	}

	public void Show(string _title, string _instruction, List<ButtonInfo> _buttons)
	{
		if (window.activeInHierarchy)
			return;

		title.text = _title;
		instruction.text = _instruction;
		if (_buttons.Count == 1)
		{
			buttons[2].gameObject.SetActive(true);
			buttons[2].onClick.AddListener(delegate() { _buttons[0].eventHandler?.Invoke(); Close(); });
			buttons[2].GetComponentInChildren<TextMeshProUGUI>().text = _buttons[0].buttonText;
		}
		else
		{
			for (int i = 0; i < _buttons.Count; i++)
			{
				ButtonInfo temp = _buttons[i];
				buttons[i].gameObject.SetActive(true);
				buttons[i].onClick.AddListener(() => { temp.eventHandler?.Invoke(); Close(); });
				buttons[i].GetComponentInChildren<TextMeshProUGUI>().text = _buttons[i].buttonText;
			}
		}

		window.SetActive(true);
	}

	private void Close()
	{
		foreach (Button b in buttons)
		{
			b.onClick.RemoveAllListeners();
			b.gameObject.SetActive(false);
		}
		window.SetActive(false);
	}
}
