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
	private List<TMP> texts;

	[SerializeField]
	private float interval = 0.1f;

	private int step = 0;

	private void Awake()
	{
		dividedNumber = new List<List<int>>();
		texts = new List<TMP>();

		var temp = transform.Find("DetailBG");
		foreach(var i in Enumerable.Range(0, 7))
		{
			dividedNumber.Add(new List<int>());
			texts.Add(temp.Find($"Criteria{i}").Find("Number").GetComponent<TMP>());
		}

		GetDividedCriteria(new List<int> {12, 3, 4567, 8, 9 });
		GetDividedCombo(5432);
		GetDividedScore(654321);

		gameObject.GetComponentInChildren<TitleAnimationEventHolder>().AnimationFinished += () => { step = 1; StartCoroutine(CriteriaCountUp()); };
	}

	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			if (step == 0)
			{
				Animator anim = GetComponentInChildren<Animator>();
				anim.Play("ResultTitleAppear", 0, 1);
				step = 1;
			}
			else if (step == 1)
			{
				//TODO stop all coroutine and finish count
				StopAllCoroutines();
				FinishCountUp();
			}
			else
			{
				AsyncSceneLoader.LoadAsyncAdditive("Base", this);
				return;
			}
		}
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

	private void FinishCountUp()
	{
		foreach (var listIndex in Enumerable.Range(0, 7))
		{
			foreach (var i in Enumerable.Range(0, dividedNumber[listIndex].Count))
			{
				char[] numberText = texts[listIndex].text.ToCharArray();
				numberText[numberText.Length - 1 - i] = dividedNumber[listIndex][i].ToString()[0];
				texts[listIndex].text = numberText.ArrayToString();
			}
		}
		step = 2;
	}
	private IEnumerator CriteriaCountUp()
	{
		foreach (var listIndex in Enumerable.Range(0, 7))
		{
			foreach (var i in Enumerable.Range(0, dividedNumber[listIndex].Count))
			{
				char[] numberText = texts[listIndex].text.ToCharArray();
				foreach (var j in Enumerable.Range(0, dividedNumber[listIndex][i] + 1))
				{
					numberText[numberText.Length - 1 - i] = j.ToString()[0];
					texts[listIndex].text = numberText.ArrayToString();
					yield return new WaitForSecondsRealtime(0.1f);
				}
			}
		}
		step = 2;
	}
}
