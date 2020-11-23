using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterClearAnimationEventHolder : MonoBehaviour
{
	public delegate void AnimationEventHandler();
	public event AnimationEventHandler AnimationFinished;

	public void AfterAnimationEvent()
	{
		AnimationFinished?.Invoke();
	}

	public void StartAnimation()
	{
		gameObject.GetComponent<Animator>().Play("AfterClear");
	}
}
