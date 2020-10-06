using System.Collections.Generic;
using UnityEngine;

public class HitRange : MonoBehaviour
{
	public static HitRange Instance { get; private set; }

	[HideInInspector]
	public int count = 4;
	[HideInInspector]
	public List<float> ranges = new List<float> {15, 10, 5, 1 };
	[HideInInspector]
	public List<Color> colors = new List<Color> { Color.red, Color.yellow, Color.blue, Color.green };

	private void Awake()
	{
		Instance = this;
	}

	public void LimitRange()
	{
		if (ranges == null || ranges.Count == 0)
			return;

		for (int i = 0; i < count-1; i++)
		{
			ranges[i] = Mathf.Max(ranges[i], ranges[i + 1]);
		}
		ranges[count-1] = Mathf.Max(ranges[count - 1], 0);
	}

	public void CountChanged(int newCount)
	{
		if (newCount < 0)
			return;

		int diff = newCount - count;
#region Increased
		if (diff > 0)
		{
			//0 -> 1
			if (newCount == 1)
			{
				ranges = new List<float> { 1 };
				colors = new List<Color> { Color.green };
			}
			else
			{
				for (int i = 0; i < diff; i++)
				{
					ranges.Insert(0, ranges[0] == 1? 5 : ranges[0] + 5);
					colors.Insert(0, new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), 1.0f));
				}
			}
		}
#endregion
#region Decreased
		else if(diff < 0)
		{
			diff *= -1;

			if (newCount == 0 ||
				diff >= count)
			{
				ranges.Clear();
				colors.Clear();
			}
			else
			{
				for (int i = 0; i < diff; i++)
				{
					ranges.RemoveAt(0);
					colors.RemoveAt(0);
				}
			}
		}
#endregion
		count = newCount;
	}
}
