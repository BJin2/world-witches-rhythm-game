using UnityEngine;

public class Character : MonoBehaviour
{
	private LineRenderer laneIndicator = null;
	private readonly Color[] colorStep = { Color.black, Color.red, Color.yellow, Color.blue, Color.green };
	[SerializeField]
	private Neuroi closestNeuroi = null;

	private int lane = -1;

	private void Awake()
	{
		laneIndicator = GetComponentInChildren<LineRenderer>();
		laneIndicator.SetPosition(0, transform.position);
		laneIndicator.SetPosition(1, transform.position + (Vector3.forward*50));
		SetLaneIndicatorColor(colorStep[0]);
	}

	private void Update()
	{
		if (IsMissing(closestNeuroi))
		{
			laneIndicator.SetPosition(1, transform.position + (Vector3.forward * 50));
			SetLaneIndicatorColor(colorStep[0]);
			closestNeuroi = Spawner.Instance.GetFirstActiveNeuroiOnLane(lane);
		}
		else
		{
			laneIndicator.SetPosition(1, closestNeuroi.transform.position);
			SetLaneIndicatorColor(colorStep[closestNeuroi.GetScore() / Neuroi.SCORE_MULTIPLIER]);
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

	}

	private void SetLaneIndicatorColor(Color toSet)
	{
		laneIndicator.startColor = toSet;
		laneIndicator.endColor = toSet;
	}
	private bool IsMissing(UnityEngine.MonoBehaviour obj)
	{
		if (obj == null)
			return true;

		try
		{
			obj.GetInstanceID();
			return !obj.gameObject.activeInHierarchy;
		}
		catch
		{
			return true;
		}
	}
}
