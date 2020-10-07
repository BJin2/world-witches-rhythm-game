using TMPro;
using UnityEngine;

public class HitResult : MonoBehaviour
{
	public static HitResult Instance { get; private set; }

	[SerializeField]
	private TextMeshProUGUI criteriaText = null;
	private string[] criteria = { "Miss", "Bad", "Nice" , "Good", "Perfect" };

	[SerializeField]
	private TextMeshProUGUI comboText = null;
	private Animator comboAnim = null;
	private int combo = 0;

	private void Awake()
	{
		Instance = this;
		comboAnim = comboText.GetComponent<Animator>();
	}

	public void Judge(int score)
	{
		int step = score / Neuroi.SCORE_MULTIPLIER;
		if (step < 3)
		{
			HideCombo();
		}
		else
		{
			IncreaseCombo();
		}
		criteriaText.text = criteria[step];
	}

	private void IncreaseCombo()
	{
		comboText.transform.parent.gameObject.SetActive(true);
		combo++;
		comboText.text = combo.ToString();
		comboAnim.Play("ComboScale",0, 0);
	}
	private void HideCombo()
	{
		comboAnim.StopPlayback();
		comboText.transform.parent.gameObject.SetActive(false);
		combo = 0;
	}
}
