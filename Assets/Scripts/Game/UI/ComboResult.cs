using UnityEngine;

[System.Serializable]
public class ComboResult : HitResultComponent
{
	private Animator comboAnim = null;
	public int MaxCombo { get; private set; }

	public override void Init()
	{
		value = 0;
		comboAnim = text.GetComponent<Animator>();
		MaxCombo = 0;
	}

	public override void Judge(int score)
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
	}

	private void IncreaseCombo()
	{
		text.transform.parent.gameObject.SetActive(true);
		value++;
		text.text = value.ToString();
		comboAnim.Play("ComboScale", 0, 0);

		if (value > MaxCombo)
			MaxCombo = value;
	}
	private void HideCombo()
	{
		comboAnim.StopPlayback();
		text.transform.parent.gameObject.SetActive(false);
		value = 0;
	}
}
