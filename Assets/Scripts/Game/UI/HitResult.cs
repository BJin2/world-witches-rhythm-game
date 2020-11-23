using TMPro;
using UnityEngine;
using System.Collections.Generic;

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

	public int MaxCombo { get; private set; }
	public List<int> NumCriteria { get; private set; }
	public int Score { get; private set; }

	private void Awake()
	{
		Instance = this;
		comboAnim = comboText.GetComponent<Animator>();
		MaxCombo = 0;
		NumCriteria = new List<int>();
		for (int i = 0; i < criteria.Length; i++)
		{
			NumCriteria.Add(0);
		}
	}

	public void Judge(int score)
	{
		Score += score;
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
		NumCriteria[step]++;
	}

	private void IncreaseCombo()
	{
		comboText.transform.parent.gameObject.SetActive(true);
		combo++;
		comboText.text = combo.ToString();
		comboAnim.Play("ComboScale",0, 0);

		if (combo > MaxCombo)
			MaxCombo = combo;
	}
	private void HideCombo()
	{
		comboAnim.StopPlayback();
		comboText.transform.parent.gameObject.SetActive(false);
		combo = 0;
	}
}
