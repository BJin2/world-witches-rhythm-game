using System;
using System.Collections;
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

[Serializable]
public class MaterialTarget
{
	public Material material;
	public Renderer renderer;
}

[ExecuteInEditMode]
public class MaterialLightHelper : MonoBehaviour
{
	public bool autoTarget = false;
	public MaterialTarget[] targets = null;

	private Material[] materialInstance = null;

    [HideInInspector]
    public bool instanceMaterial = true;
    [HideInInspector]
    public bool manualLights = false;
    [HideInInspector]
    public int maxLights = 6;
    [HideInInspector]
    public List<Light> lights = null;
	private Dictionary<int, LightData> lightDatas = null;	

	private void OnValidate()
	{
		Init();
	}

	private void Start()
	{
		Init();
	}

	private void Update()
	{
		//Update light only when it's not playing
		if(!Application.isEditor && !Application.isPlaying)
			UpdateLights();
		UpdateMaterial();
	}

	private void Init()
	{
		//Material and Renderer (Helper targets)
		if (targets.Length == 0)
		{
			if (!autoTarget)
				return;

			Renderer[] renderers = GetComponentsInChildren<Renderer>();
			targets = new MaterialTarget[renderers.Length];
			for (int i = 0; i < renderers.Length; i++)
			{
				targets[i] = new MaterialTarget();
				targets[i].renderer = renderers[i];
				targets[i].material = null;
			}
		}

		//Material Instance
		materialInstance = new Material[targets.Length];

		for (int i = 0; i < targets.Length; i++)
		{
			if (targets[i].material != null)
			{
				if (instanceMaterial)
				{
					materialInstance[i] = new Material(targets[i].material);
					materialInstance[i].name = targets[i].material.name + "(Copy)";
				}
				else
				{
					materialInstance[i] = targets[i].material;
				}
			}
		}

		Update();
	}

	//Call manually when light has added or removed dynamically
	public void UpdateLights()
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

		//Compare and remove light data
		/*
		 * About existingLightID initialization timing.
		 *	1. ID of newly added lights will definitely be in newLightID 
		 *		so it is not necessary to add the same thing to existingLightID
		 *	2. Less items lead to less iteration(actully it's so small number of items so it barely affect the performance)
		 *	3. List declarations at the same place for better legibility
		 */
		List<int> newLightID = new List<int>();
		List<int> existingLightID = new List<int>(lightDatas.Keys);

		List<int> toRemoveIndex = new List<int>();
		int index = -1;

		//Add new light data
		foreach (Light light in lights)
		{
			index++;
			int id;

			//No further process if the light is removed (this part is for manual light only)
			if (IsMissing(light, out id))
			{
				toRemoveIndex.Add(index);
				continue;
			}

			newLightID.Add(id);
			if (!lightDatas.ContainsKey(id))
			{
				lightDatas.Add(id, new LightData(light));
			}
		}

		if (Application.isPlaying)
		{
			foreach (int i in toRemoveIndex)
			{
				lights.RemoveAt(i);
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

	private LightData UpdateLightData(LightData data)
	{
		return new LightData();
	}

	private void UpdateMaterial()
	{
		if (targets.Length == 0)
			return;


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
}
