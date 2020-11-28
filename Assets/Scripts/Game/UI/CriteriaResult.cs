using System.Collections.Generic;

[System.Serializable]
public class CriteriaResult : HitResultComponent
{
	private readonly string[] criteria = { "Miss", "Bad", "Nice", "Good", "Perfect" };
	public List<int> NumCriteria { get; private set; }

	public override void Init()
	{
		NumCriteria = new List<int>();
		for (int i = 0; i < criteria.Length; i++)
		{
			NumCriteria.Add(0);
		}
	}

	public override void Judge(int score)
	{
		int step = score / Neuroi.SCORE_MULTIPLIER;
		text.text = criteria[step];
		NumCriteria[step]++;
	}
}
