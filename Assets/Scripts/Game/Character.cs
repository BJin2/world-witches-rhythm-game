using UnityEngine;

public class Character : MonoBehaviour
{
	private int lane = -1;

	private LineRenderer laneIndicator = null;
	private readonly Color[] colorStep = { Color.black, Color.red, Color.yellow, Color.blue, Color.green };
	private int colorStepIndex = 0;
	private Neuroi closestNeuroi = null;

	[SerializeField][Range(0.0f, 5.0f)]
	private float shieldCountdown = 0.3f;
	private float shieldTimer = 0.0f;
	private GameObject shield = null;

	[SerializeField][Range(0.0f, 5.0f)]
	private float muzzleCountdown = 0.1f;
	private float muzzleTimer = 0.0f;
	private GameObject muzzleFlash = null;
	

	private void Awake()
	{
		laneIndicator = GetComponentInChildren<LineRenderer>();
		laneIndicator.SetPosition(0, transform.position);
		laneIndicator.SetPosition(1, transform.position + (Vector3.forward*50));
		SetLaneIndicatorColor(colorStep[0]);

		GameObject[] shields = GameObject.FindGameObjectsWithTag("Shield");
		foreach (GameObject sh in shields)
		{
			if (sh.transform.parent == transform)
			{
				shield = sh;
				break;
			}
		}

		muzzleFlash = transform.Find("MuzzleFlash").gameObject;
	}

	private void Update()
	{
		/*/
		LaneIndicator();
		/*/
		LaneIndicatorOnShoot();
		//*/

		ShieldCountdown();
		ShootCountdown();
	}

	public void SetLane(int l)
	{
		lane = l;
	}

	public void Shoot()
	{
		muzzleTimer = muzzleCountdown;
		if (!Flight.IsMissing(closestNeuroi))
			colorStepIndex = closestNeuroi.GetScore() / Neuroi.SCORE_MULTIPLIER;
		else
			colorStepIndex = 0;
	}

	public void Shield()
	{
		shieldTimer = shieldCountdown;
	}

	private void LaneIndicator()
	{
		//Double check if it's missing(getting the closest one does not guarantee existance so closest one from flight class can be null or missing)
		if (Flight.IsMissing(closestNeuroi))
		{
			laneIndicator.SetPosition(1, transform.position + (Vector3.forward * 50));
			SetLaneIndicatorColor(colorStep[0]);
		}
		else
		{
			laneIndicator.SetPosition(1, closestNeuroi.transform.position);
			SetLaneIndicatorColor(colorStep[closestNeuroi.GetScore() / Neuroi.SCORE_MULTIPLIER]);
		}
	}
	private void SetLaneIndicatorColor(Color toSet)
	{
		laneIndicator.startColor = toSet;
		laneIndicator.endColor = toSet;
	}
	public void SetClosest(Neuroi closest)
	{
		closestNeuroi = closest;
	}

	private void ShieldCountdown()
	{
		if (shieldTimer > 0)
		{
			shieldTimer -= Time.deltaTime;
			if (!shield.activeInHierarchy)
			{
				shield.SetActive(true);
			}
		}
		else
		{
			if (shield.activeInHierarchy)
			{
				shield.SetActive(false);
			}
		}
	}
	private void ShootCountdown()
	{
		if (muzzleTimer > 0)
		{
			muzzleTimer -= Time.deltaTime;
			if (!muzzleFlash.activeInHierarchy)
			{
				muzzleFlash.SetActive(true);
			}
		}
		else
		{
			if (muzzleFlash.activeInHierarchy)
			{
				muzzleFlash.SetActive(false);
				colorStepIndex = 0;
			}
		}
	}
	private void LaneIndicatorOnShoot()
	{
		SetLaneIndicatorColor(colorStep[colorStepIndex]);

		if (Flight.IsMissing(closestNeuroi))
		{
			laneIndicator.SetPosition(1, transform.position + (Vector3.forward * 50));
		}
		else
		{
			laneIndicator.SetPosition(1, closestNeuroi.transform.position);
		}
		
	}
}
