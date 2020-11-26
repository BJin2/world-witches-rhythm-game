using UnityEngine;

//long neuroi and regular neuroi classes will inherit this abstract class
public abstract class Neuroi : MonoBehaviour
{
	//All neurois share the same speed and hit position
	public static float Speed { get; set; }
	public static float HitPoisition { get; private set; }
	public const int SCORE_MULTIPLIER = 3;
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
				closestStep = i+1;
			else
				break;
		}

		score = closestStep * SCORE_MULTIPLIER;
		Debug.Log(score);
		if (transform.position.z <= 0)
		{
			score = 0;
			Explode();
			NeuroiManager.Instance.NeuroiCrashed(this);
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
		HitResult.Instance.Judge(score);
		gameObject.SetActive(false);
	}
	public bool ShootDown()
	{
		//Ignore when it's too far from hit position
		if (score <= 0)
			return false;
		Explode();
		return true;
	}

	public void SetLane(int l)
	{
		Lane = l;
	}
	public int GetScore() => score;

	public static float FindHitPosition()
	{
		GameObject temp = GameObject.FindGameObjectWithTag("Hit");
		HitPoisition = temp.transform.position.z;
		return HitPoisition;
	}
}