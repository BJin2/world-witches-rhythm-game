using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentChildTest : MonoBehaviour
{
	public MeshRenderer[] renderers;
	private void Awake()
	{
		renderers = GetComponentsInChildren<MeshRenderer>();
	}
}
