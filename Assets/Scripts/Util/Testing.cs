using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
	public Material[] mat;
	private Material[] temp;

	private void Start()
	{
		temp = new Material[mat.Length];
		for (int i = 0; i < mat.Length; i++)
		{
			temp[i] = new Material(mat[i])
			{
				name = mat[i].name + "(Copy)"
			};
		}

		GetComponent<Renderer>().sharedMaterials = new Material[temp.Length];

		GetComponent<Renderer>().sharedMaterials = temp;
		GetComponent<MeshRenderer>().sharedMaterials[1] = null;
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			temp[0].color = Color.red;
		}
	}
}
