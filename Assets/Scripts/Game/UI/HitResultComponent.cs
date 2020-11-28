using UnityEngine;

using TMP = TMPro.TextMeshProUGUI;

[System.Serializable]
public abstract class HitResultComponent
{
	public TMP text = null;
	protected int value = int.MaxValue;

	public abstract void Init();

	public abstract void Judge(int score);
}
