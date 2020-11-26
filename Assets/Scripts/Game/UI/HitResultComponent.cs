using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMP = TMPro.TextMeshProUGUI;

public abstract class HitResultComponent
{
	[SerializeField]
	protected TMP text;
	[SerializeField]
	protected int value;

	protected abstract void Judge(int score);
}
