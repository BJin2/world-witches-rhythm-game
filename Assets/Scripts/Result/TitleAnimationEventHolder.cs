using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleAnimationEventHolder : MonoBehaviour
{
	public delegate void AnimationDone();
	public event AnimationDone AnimationFinished;
	public void AnimationFinishedEvent()
	{
		AnimationFinished?.Invoke();
	}
}
