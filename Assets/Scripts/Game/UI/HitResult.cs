using TMPro;
using UnityEngine;
using System.Collections.Generic;

using TMP = TMPro.TextMeshProUGUI;

public class HitResult : MonoBehaviour
{
	public static HitResult Instance { get; private set; }

	[SerializeField]
	private TMP criteriaText = null;
	private string[] criteria = { "Miss", "Bad", "Nice" , "Good", "Perfect" };

	[SerializeField]
	private TMP comboText = null;
	private Animator comboAnim = null;
	private int combo = 0;

	[SerializeField]
	private UnityEngine.UI.Slider scoreBar = null;
	[SerializeField]
	private TMP scoreText = null;
	[SerializeField]
	private Sprite sRankImage = null;
	private float sRankStandard = 0;

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
		AddScore(score);
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

	private void AddScore(int amount)
	{
		if (sRankStandard == 0)
		{
			sRankStandard = Neuroi.SCORE_MULTIPLIER * criteria.Length * NeuroiManager.Instance.TotalNeuroi * 0.6f;
		}
		Score += amount;
		scoreText.text = Score.ToString();
		scoreBar.value = (float)Score/sRankStandard;
		if (scoreBar.value < 1)
		{
			scoreBar.fillRect.GetComponent<UnityEngine.UI.Image>().color = new Color(0.5f, 1, scoreBar.value);
		}
		else
		{
			scoreBar.fillRect.GetComponent<UnityEngine.UI.Image>().sprite = sRankImage;
		}
	}
}
