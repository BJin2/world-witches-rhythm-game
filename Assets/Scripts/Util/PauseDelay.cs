using System.Collections;
using UnityEngine;
using TMPro;

public class PauseDelay : MonoBehaviour
{
	public static PauseDelay Instance { get; private set; }

	public delegate void DelayDone();

	[SerializeField]
	private GameObject pauseCanvas = null;
	[SerializeField]
	private TextMeshProUGUI countdownText = null;
	[SerializeField]
	private GameObject pauseBG = null;
	public event DelayDone AfterDelay = null;

	//To prevent overlapped delay
	private bool delayRunning = false;

	private void Awake()
	{
		Instance = this;
	}

	public void DarkBG(bool on)
	{
		pauseBG.SetActive(on);
	}

	public void Delay(float delay)
	{
		if (delayRunning)
			return;

		delayRunning = true;
		pauseCanvas.SetActive(true);
		StartCoroutine(DelayWithText(delay));
	}

	private IEnumerator DelayWithText(float delay)
	{
		while (delay > 0)
		{
			countdownText.text = ((int)delay).ToString();
			yield return new WaitForSecondsRealtime(1);
			delay--;
		}

		pauseCanvas.SetActive(false);
		AfterDelay?.Invoke();
		AfterDelay = null;
		delayRunning = false;
	}
}
