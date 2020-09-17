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
	
	private Vector3 center = Vector3.zero;//average vector of target positions
	private Material[] materialInstance = null;
	private Dictionary<int, LightData> lightDatas = null;

	[HideInInspector]
    public bool instanceMaterial = true;
    [HideInInspector]
    public int maxLights = 6;
	[HideInInspector]
	public bool manualLights = false;
	[HideInInspector]
    public List<Light> lights = null;

	[HideInInspector]
	public bool raycast = true;
	[HideInInspector]
	public LayerMask raycastMask = new LayerMask();
	[HideInInspector]
	public float raycastFadeSpeed = 10.0f;

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
		if (targets.Length == 0)
			return;

		center = CalculateCenter();
		//Update light only when it's not playing
		if(Application.isEditor && !Application.isPlaying)
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
				targets[i] = new MaterialTarget
				{
					renderer = renderers[i],
					material = null
				};
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
					materialInstance[i] = new Material(targets[i].material)
					{
						name = targets[i].material.name + "(Copy)"
					};
				}
				else
				{
					materialInstance[i] = targets[i].material;
				}

				if (targets[i].renderer)
					targets[i].renderer.sharedMaterial = materialInstance[i];
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

			//No further process if the light is removed (this part is for manual light only)
			if (IsMissing(light, out int id))
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

	void UpdateMaterial()
	{
		if (targets.Length == 0)
			return;

		// Sort lights by brightness
		List<LightData> sortedLights = new List<LightData>();
		if (lightDatas != null)
		{
			foreach (LightData lightData in lightDatas.Values)
			{
				sortedLights.Add(UpdateLightData(lightData));
			}
		}
		sortedLights.Sort((x, y) => {
			float yBrightness = y.color.grayscale * y.atten;
			float xBrightness = x.color.grayscale * x.atten;
			return yBrightness.CompareTo(xBrightness);
		});

		// Apply lighting
		int i = 1;
		foreach (LightData lightData in sortedLights)
		{
			if (i > maxLights) break;
			if (lightData.atten <= Mathf.Epsilon) break;

			// Use color Alpha to pass attenuation data
			Color color = lightData.color;
			color.a = Mathf.Clamp(lightData.atten, 0.01f, 0.99f); // UV might wrap around if attenuation is >1 or 0<
			foreach (Material instance in materialInstance)
			{
				if (instance == null)
					continue;
				instance.SetVector($"_L{i}_dir", lightData.direction.normalized);
				instance.SetColor($"_L{i}_color", color);
			}
			i++;
		}

		// Turn off the remaining light slots
		while (i <= maxLights)
		{
			foreach (Material instance in materialInstance)
			{
				if (instance == null)
					continue;
				instance.SetVector($"_L{i}_dir", Vector3.up);
				instance.SetColor($"_L{i}_color", Color.black);
			}
			i++;
		}

		// Store updated light data
		foreach (LightData lightData in sortedLights)
		{
			lightDatas[lightData.id] = lightData;
		}
	}

	float TestInView(Vector3 dir, float dist)
	{
		if (!raycast) return 1.1f;
		if (Physics.Raycast(center, dir, out RaycastHit hit, dist, raycastMask))
		{
			Debug.DrawRay(center, dir.normalized * hit.distance, Color.red);
			return -0.1f;
		}
		else
		{
			Debug.DrawRay(center, dir.normalized * dist, Color.green);
			return 1.1f;
		}
	}

	// Ref - Light Attenuation calc: https://forum.unity.com/threads/light-attentuation-equation.16006/#post-3354254
	float CalcAttenuation(float dist)
	{
		return Mathf.Clamp01(1.0f / (1.0f + 25f * dist * dist) * Mathf.Clamp01((1f - dist) * 5f));
	}

	LightData UpdateLightData(LightData lightData)
	{
		Light light = lightData.light;
		float inView = 1.1f;
		float dist;

		if (!light.isActiveAndEnabled)
		{
			lightData.atten = 0f;
			return lightData;
		}

		switch (light.type)
		{
			case LightType.Directional:
				lightData.direction = light.transform.forward * -1f;
				inView = TestInView(lightData.direction, 100f);
				lightData.color = light.color * light.intensity;
				lightData.atten = 1f;
				break;

			case LightType.Point:
				lightData.direction = light.transform.position - center;
				dist = Mathf.Clamp01(lightData.direction.magnitude / light.range);
				inView = TestInView(lightData.direction, lightData.direction.magnitude);
				lightData.atten = CalcAttenuation(dist);
				lightData.color = light.color * lightData.atten * light.intensity * 0.1f;
				break;

			case LightType.Spot:
				lightData.direction = light.transform.position - center;
				dist = Mathf.Clamp01(lightData.direction.magnitude / light.range);
				float angle = Vector3.Angle(light.transform.forward * -1f, lightData.direction.normalized);
				float inFront = Mathf.Lerp(0f, 1f, (light.spotAngle - angle * 2f) / lightData.direction.magnitude); // More edge fade when far away from light source
				inView = inFront * TestInView(lightData.direction, lightData.direction.magnitude);
				lightData.atten = CalcAttenuation(dist);
				lightData.color = light.color * lightData.atten * light.intensity * 0.05f;
				break;

			default:
				Debug.Log(light.type + " not supported");
				lightData.atten = 0f;
				break;
		}

		// Slowly fade lights on and off
		float fadeSpeed = (Application.isEditor && !Application.isPlaying)
			? raycastFadeSpeed / 60f
			: raycastFadeSpeed * Time.deltaTime;

		lightData.inView = Mathf.Lerp(lightData.inView, inView, fadeSpeed);
		lightData.color *= Mathf.Clamp01(lightData.inView);

		return lightData;
	}

	private Vector3 CalculateCenter()
	{
		Vector3 result = transform.position;

		foreach (MaterialTarget mt in targets)
		{
			if (mt.renderer == null)
				continue;
			result += mt.renderer.transform.position;
		}

		return (result / (targets.Length+1));
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
}
