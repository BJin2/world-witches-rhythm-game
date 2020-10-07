using UnityEngine;

//long neuroi and regular neuroi classes will inherit this abstract class
public abstract class Neuroi : MonoBehaviour
{
	//All neurois share the same speed and hit position
	public static float Speed { get; set; }
	public static float HitPoisition { get; private set; }

	private int score = 0;
	public int Lane { get; private set; }

	[SerializeField]
	private GameObject piece = null;

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
			Explode();
			Spawner.Instance.NeuroiCrashed(this);
		}
	}

	private void Explode()
	{
		if (piece != null)
		{
			piece.SetActive(true);
			piece.transform.parent = null;
			Destroy(piece, 0.3f);
		}
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
		Lane = l;
	}

	public static float FindHitPosition()
	{
		GameObject temp = GameObject.FindGameObjectWithTag("Hit");
		HitPoisition = temp.transform.position.z;
		return HitPoisition;
	}
}