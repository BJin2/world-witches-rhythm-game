using TMPro;
using UnityEngine;
using System.Collections.Generic;

using TMP = TMPro.TextMeshProUGUI;

public class HitResult : MonoBehaviour
{
	public static HitResult Instance { get; private set; }

	private List<HitResultComponent> hitResultComponents;

	[SerializeField]
	private CriteriaResult criteriaResult = null;
	[SerializeField]
	private ScoreResult scoreResult = null;
	[SerializeField]
	private ComboResult comboResult = null;


	private void Awake()
	{
		Instance = this;

		hitResultComponents = new List<HitResultComponent>();
		hitResultComponents.Add(criteriaResult);
		hitResultComponents.Add(scoreResult);
		hitResultComponents.Add(comboResult);

		foreach (HitResultComponent hrc in hitResultComponents)
		{
			if (hrc == null || hrc.text == null)
				continue;	
			hrc.Init();
		}
	}

	public void Judge(int score)
	{
		foreach (HitResultComponent hrc in hitResultComponents)
		{
			if (hrc == null || hrc.text == null)
				continue;
			hrc.Judge(score);
		}
	}

	public T GetHitResultComponent<T>() where T : HitResultComponent
	{
		if (hitResultComponents == null)
			return null;

		foreach (HitResultComponent hrc in hitResultComponents)
		{
			if (hrc is T)
			{
				return (T)hrc;
			}
		}
		return null;
	}
}
