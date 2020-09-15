using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public struct LightData
{
    public int id;
    public Light light;
    public Vector3 direction;
    public Color color;
    public float atten;
    public float inView;

	public LightData(Light newLight)
	{
		id = newLight.GetInstanceID();
		light = newLight;
		direction = Vector3.zero;
		color = Color.black;
		color.a = 0.01f;
		atten = 0.0f;
		inView = 1.1f;
	}
}

[ExecuteInEditMode]
public class MaterialLightHelper : MonoBehaviour
{
    public Material material = null;

    [HideInInspector]
    public bool instanceMaterial = true;
    [HideInInspector]
    public bool manualLights = false;
    [HideInInspector]
    public int maxLights = 6;
    [HideInInspector]
    public Light[] lights;


	private void OnValidate()
	{
		
	}

	private void Start()
	{
		
	}

	private void Update()
	{
		
	}

	private void Init()
	{

	}
	private void GetLights()
	{

	}
	private void UpdateMaterial()
	{

	}
	}
