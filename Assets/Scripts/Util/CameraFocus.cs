using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CameraFocus : MonoBehaviour
{
	[SerializeField]
	private LayerMask focusLayer = new LayerMask();
	private float far = 100.0f;

	[SerializeField]
	private Transform target = null;

	[SerializeField]
	private Volume volume;
	private DepthOfField focus;

	private void Start()
	{
		far = GetComponent<Camera>().farClipPlane;
		volume.profile.TryGet<DepthOfField>(out focus);
	}

	private void Update()
	{
		Vector3 direction = transform.forward;

		if (target)
			direction = (target.position - transform.position).normalized;

		if (Physics.Raycast(transform.position, direction, out RaycastHit hit, far, focusLayer))
		{
			//focus.focusDistance.value = hit.distance;
			focus.focusDistance.value = Mathf.Lerp(focus.focusDistance.value, hit.distance, 2 * Time.deltaTime);
			Debug.DrawRay(transform.position, direction * hit.distance, Color.red);
		}
		else
		{
			Debug.DrawRay(transform.position, direction * far, Color.green);
		}
	}
}
