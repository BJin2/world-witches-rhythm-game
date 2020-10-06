using UnityEngine;

//long neuroi and regular neuroi classes will inherit this abstract class
public abstract class Neuroi : MonoBehaviour
{
	//All neurois share the same speed and hit position
	public static float Speed { get; set; }
	public static float HitPoisition { get; private set; }

	private int score;

	protected virtual void Update()
	{
		transform.Translate(transform.forward * Speed * Time.deltaTime * -1);
		//TODO start calculating score within certain range
		float dist = Mathf.Abs(transform.position.z - HitPoisition);
		int closestStep = 0;
		for (int i = 0; i < HitRange.Instance.count; i++)
		{
			if (dist < HitRange.Instance.ranges[i])
				closestStep = i;
			else
				break;
		}

		score = closestStep * 3;
	}

	public void Explode()
	{
		//TODO explosion particle
		//TODO increase score
		//TODO combo indicator && 
	}

	public static float FindHitPosition()
	{
		GameObject temp = GameObject.FindGameObjectWithTag("Hit");
		HitPoisition = temp.transform.position.z;
		return HitPoisition;
	}
}