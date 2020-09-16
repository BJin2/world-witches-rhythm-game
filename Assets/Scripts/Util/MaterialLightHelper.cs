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
		//UpdateLights();
		//UpdateMaterial();
	}

	private void Init()
	{

	}
	private void UpdateLights()
	{
		if (lightDatas == null)
		{
			lightDatas = new Dictionary<int, LightData>();
		}

		if (manualLights) // Does not update lights automatically
		{
			foreach (Light light in lights)
			{
				if (IsMissing(light))
				{
					lights.Remove(light);
					continue;
				}

				int id = light.GetInstanceID();
				if (!lightDatas.ContainsKey(id))
				{
					lightDatas.Add(id, new LightData(light));
				}
			}
		}
		else
		{
			//Find lights
			lights = FindObjectsOfType<Light>().ToList();

			//Add if not in light datas
			foreach (Light light in lights)
			{
				int id = light.GetInstanceID();
				if (!lightDatas.ContainsKey(id))
				{
					lightDatas.Add(id, new LightData(light));
				}
			}
		}

		//Remove from light datas if light does not exist
		foreach (KeyValuePair<int, LightData> ld in lightDatas)
		{
			if (!lights.Contains(ld.Value.light))
			{
				lightDatas.Remove(ld.Key);
			}
		}
	}
	private void UpdateMaterial()
	{

	}

	private bool IsMissing(UnityEngine.Object obj)
	{
		try
		{
			obj.GetInstanceID();
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
