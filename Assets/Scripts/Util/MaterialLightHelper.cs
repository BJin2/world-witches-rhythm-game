﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
    public List<Light> lights = null;
	private Dictionary<int, LightData> lightDatas = null;

	private delegate void ForEachLight();

	private void OnValidate()
	{
		
	}

	private void Start()
	{
		
	}

	private void Update()
	{
		UpdateLights();
		//UpdateMaterial();
	}

	private void Init()
	{

	}
	private void UpdateLights()
	{
		//initialize lightDatas dictionary if not initialized
		if (lightDatas == null)
		{
			lightDatas = new Dictionary<int, LightData>();
		}

		if (!manualLights) // Update lights
		{
			lights = FindObjectsOfType<Light>().ToList();
		}

		//Compare and organize light data
		List<int> newLightID = new List<int>();
		List<int> existingLightID = new List<int>(lightDatas.Keys);

		//Add new light data
		foreach (Light light in lights)
		{
			int id;

			//No further process if the light is removed (this part is for manual light only)
			if (IsMissing(light, out id))
			{
				continue;
			}

			newLightID.Add(id);
			if (!lightDatas.ContainsKey(id))
			{
				lightDatas.Add(id, new LightData(light));
			}
		}

		//Remove light datas if light does not exist
		foreach (int id in existingLightID)
		{
			if (!newLightID.Contains(id))
			{
				lightDatas.Remove(id);
			}
		}
	}
	private void UpdateMaterial()
	{

	}

	private bool IsMissing(UnityEngine.Object obj, out int id)
	{
		id = int.MinValue;

		if (obj == null)
			return true;

		try
		{
			id = obj.GetInstanceID();
			return false;
		}
		catch
		{	
			return true;
		}
	}

	private bool IsMissing(UnityEngine.Object obj)
	{
		if (obj == null)
			return true;

		try
		{
			return false;
		}
		catch
		{
			return true;
		}
	}

	private void EachLight(ForEachLight forEachLight)
	{
		foreach (Light light in lights)
		{
			forEachLight();
		}
	}
}
