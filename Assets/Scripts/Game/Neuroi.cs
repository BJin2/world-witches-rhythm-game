using UnityEngine;

//long neuroi and regular neuroi classes will inherit this abstract class
public abstract class Neuroi : MonoBehaviour
{
	//All neurois share the same speed and hit position
	public static float Speed { get; set; }
	public static float HitPoisition { get; private set; }

	private int score = 0;
	public int lane { get; private set; }

	protected virtual void Update()
	{
		transform.Translate(transform.forward * Speed * Time.deltaTime * -1);
		
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

		if (transform.position.z <= 0)
		{
			ShootDown();
			Spawner.Instance.NeuroiCrashed(this);
		}
	}

	private void Explode()
	{
		gameObject.SetActive(false);
	}
	public bool ShootDown()
	{
		if (score <= 0)
			return false;
		//TODO explosion particle
		//TODO increase score
		//TODO combo indicator && result text(miss bad nice good perfect)
		Explode();
		return true;
	}

	public void SetLane(int l)
	{
		lane = l;
	}

	public static float FindHitPosition()
	{
		GameObject temp = GameObject.FindGameObjectWithTag("Hit");
		HitPoisition = temp.transform.position.z;
		return HitPoisition;
	}
}