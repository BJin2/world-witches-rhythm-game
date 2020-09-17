using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentChildTest : MonoBehaviour
{
	public Renderer[] renderers;
	private void Awake()
	{
		renderers = GetComponentsInChildren<Renderer>();
	}
}
