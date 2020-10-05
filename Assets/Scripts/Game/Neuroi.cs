using UnityEngine;

public abstract class Neuroi : MonoBehaviour
{
	//big neuroi and small neuroi classes will inherit this abstract class
	public static float Speed { get; set; }
	public static float HitPoisition { get; private set; }
	private int score;

	protected virtual void Update()
	{
		transform.Translate(transform.forward * Speed * Time.deltaTime * -1);
	}

	public virtual void Shoot()
	{
		Debug.Log("Laser");
	}

	public static float FindHitPosition()
	{
		GameObject temp = GameObject.FindGameObjectWithTag("Hit");
		HitPoisition = temp.transform.position.z;
		return HitPoisition;
	}
}