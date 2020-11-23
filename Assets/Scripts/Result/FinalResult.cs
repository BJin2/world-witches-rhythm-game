using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using TMP = TMPro.TextMeshProUGUI;

public class FinalResult : MonoBehaviour
{
	private List<List<int>> dividedNumber;

	[SerializeField]
	private TMP test;

	private void Awake()
	{
		dividedNumber = new List<List<int>>();
		foreach(var _ in Enumerable.Range(0, 7))
		{
			dividedNumber.Add(new List<int>());
		}

		GetDividedCriteria(new List<int> {12, 3, 4567, 8, 9 });
		GetDividedCombo(5432);
		GetDividedScore(4321);
		StartCoroutine(CountUpDelay(test, dividedNumber[0]));
	}

	private void GetDividedCriteria(List<int> criteria)
	{
		foreach (var i in Enumerable.Range(0, criteria.Count))
		{
			//Preserve original value
			int target = criteria[i];

			DivideNumber(target, i);
		}
	}
	private void GetDividedCombo(int combo)
	{
		DivideNumber(combo, 5);
	}
	private void GetDividedScore(int score)
	{
		DivideNumber(score, 6);
	}
	private void DivideNumber(int num, int index)
	{
		while (num != 0)
		{
			dividedNumber[index].Add(num % 10);
			num /= 10;
		}
	}

	private IEnumerator CountUpDelay(TMP text, List<int> list)
	{
		foreach (var i in Enumerable.Range(0, list.Count))
		{
			char[] numberText = text.text.ToCharArray();
			foreach (var j in Enumerable.Range(0, list[i] + 1))
			{
				numberText[numberText.Length - 1 - i] = j.ToString()[0];
				text.text = numberText.ArrayToString();
				yield return new WaitForSecondsRealtime(0.1f);
			}
		}
	}
}
