using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitRange : MonoBehaviour
{
	public float[] ranges = {15, 10, 4, 1 };
	public Color[] colors = { Color.red, Color.yellow, Color.blue, Color.green };

	public void LimitRange()
	{
		for (int i = 0; i < 3; i++)
		{
			ranges[i] = Mathf.Max(ranges[i], ranges[i + 1]);
		}
		ranges[3] = Mathf.Max(ranges[3], 0);
	}
}
