using UnityEngine;
using TMPro;

public class OnScreenLogger : MonoBehaviour
{
    public TextMeshProUGUI message;
    public TextMeshProUGUI callstack;

	private void Awake()
	{
		Application.logMessageReceived += HandleLog;
	}
	void HandleLog(string logString, string stackTrace, LogType type)
	{
		if (message == null || callstack == null)
			return;

		message.text = logString;
		callstack.text = stackTrace;
	}
}
