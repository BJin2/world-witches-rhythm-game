using UnityEngine;

[System.Serializable]
public class ScoreResult : HitResultComponent
{
	public UnityEngine.UI.Slider scoreBar = null;
	public Sprite sRankImage = null;
	private float sRankStandard = 0;

	public int Score
	{
		get
		{
			return value;
		}
	}

	public override void Init()
	{
		value = 0;
		return;
	}

	public override void Judge(int score)
	{
		AddScore(score);
	}

	private void AddScore(int amount)
	{
		if (sRankStandard == 0)
		{
			sRankStandard = Neuroi.SCORE_MULTIPLIER * (HitRange.Instance.count + 1) * NeuroiManager.Instance.TotalNeuroi * 0.6f;
		}
		value += amount;
		text.text = Score.ToString();
		scoreBar.value = ((float)Score) / sRankStandard;
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
