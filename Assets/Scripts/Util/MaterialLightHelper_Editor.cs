using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

#if UNITY_EDITOR

[CustomEditor(typeof(MaterialLightHelper))]
public class MaterialLightHelper_Editor : Editor
{
	private new SerializedObject serializedObject;
	private SerializedProperty serializedProperty;

	private void OnEnable()
	{
		serializedObject = new SerializedObject(target);
	}

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		serializedObject.Update();
		MaterialLightHelper script = (MaterialLightHelper)target;

		if (script.material)
		{
			script.instanceMaterial = EditorGUILayout.Toggle(new GUIContent("Instance Material", "Using instancd of the material(no effect on the original)"), script.instanceMaterial);
			script.maxLights = EditorGUILayout.IntField(new GUIContent("Maximun Lights", "Maximum number of lights"), script.maxLights);
			script.manualLights = EditorGUILayout.Toggle(new GUIContent("Manual Lights", "Manually assign lights"), script.manualLights);

			if (script.manualLights)
			{
				serializedProperty = serializedObject.FindProperty("lights");
				EditorGUI.BeginChangeCheck();
				EditorGUILayout.PropertyField(serializedProperty, new GUIContent("Lights"), true);

				EditorGUI.indentLevel = 1;
				EditorGUILayout.PropertyField(serializedProperty.FindPropertyRelative("Array.size"));
				for (int i = 0; i < serializedProperty.arraySize; i++)
				{
					EditorGUILayout.PropertyField(serializedProperty.GetArrayElementAtIndex(i));
				}
				if(EditorGUI.EndChangeCheck())
					serializedObject.ApplyModifiedProperties();
			}
		}
	}
}
#endif