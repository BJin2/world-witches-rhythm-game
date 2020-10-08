using UnityEngine;

public class Character : MonoBehaviour
{
	private LineRenderer laneIndicator = null;
	private readonly Color[] colorStep = { Color.black, Color.red, Color.yellow, Color.blue, Color.green };
	private Neuroi closestNeuroi = null;

	private int lane = -1;

	[SerializeField][Range(0.0f, 5.0f)]
	private float shieldCountdown = 0.3f;
	private float shieldTimer = 0.0f;
	private GameObject shield = null;

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
	}

	private void Update()
	{
		LaneIndicator();

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

	public void SetLane(int l)
	{
		lane = l;
	}

	public void Shoot()
	{

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
}
