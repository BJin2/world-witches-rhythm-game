﻿using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif

#if UNITY_EDITOR
[CustomEditor(typeof(CameraMovement))]
public class CameraMovement_Editor : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		CameraMovement script = (CameraMovement)target;

		script.controlType = (CameraMovement.ControlType)EditorGUILayout.EnumPopup(new GUIContent("Control Type", "Select method of camera control"), script.controlType);
		script.obstacleLayer = EditorGUILayout.MaskField(new GUIContent("Raycast Mask"), InternalEditorUtility.LayerMaskToConcatenatedLayersMask(script.obstacleLayer), InternalEditorUtility.layers);
		EditorGUILayout.Space();

		script.sensitivity = EditorGUILayout.FloatField(new GUIContent("Sensitivity", "Look sensitivity"), script.sensitivity > 0 ? script.sensitivity : 0);
		script.invertY = EditorGUILayout.Toggle(new GUIContent("Invert Y", "Inverted y input"), script.invertY);
		if (script.invertY)
		{
			script.invertYMultiplier = -1;
		}
		else
		{
			script.invertYMultiplier = 1;
		}
		EditorGUILayout.Space();

		script.target = (Transform)EditorGUILayout.ObjectField(script.target, typeof(Transform));
		EditorGUILayout.Space();

		script.xRotation = EditorGUILayout.Toggle(new GUIContent("X Rotation", "Enable rotation on X axis"), script.xRotation);
		if (script.xRotation)
		{
			EditorGUI.indentLevel = 1;
			float max = script.xAxisMaxLimit > 0 ? script.xAxisMaxLimit : 0;
			max = max < 180 ? max : 180;
			float min = script.xAxisMinLimit < 0 ? script.xAxisMinLimit : 0;
			min = min > -180 ? min : -180;
			script.xAxisMaxLimit = EditorGUILayout.FloatField(new GUIContent("X Axis Maximun Limit", "Maximun degree for X axis rotation"), max);
			script.xAxisMinLimit = EditorGUILayout.FloatField(new GUIContent("X Axis Minimun Limit", "Minimun degree for X axis rotation"), min);
		}
		EditorGUILayout.Space();

		EditorGUI.indentLevel = 0;
		script.yRotation = EditorGUILayout.Toggle(new GUIContent("Y Rotation", "Enable rotation on Y axis"), script.yRotation);
		if (script.yRotation)
		{
			EditorGUI.indentLevel = 1;
			float max = script.yAxisMaxLimit > 0 ? script.yAxisMaxLimit : 0;
			max = max < 180 ? max : 180;
			float min = script.yAxisMinLimit < 0 ? script.yAxisMinLimit : 0;
			min = min > -180 ? min : -180;
			script.yAxisMaxLimit = EditorGUILayout.FloatField(new GUIContent("Y Axis Maximun Limit", "Maximun degree for Y axis rotation"), max);
			script.yAxisMinLimit = EditorGUILayout.FloatField(new GUIContent("Y Axis Minimun Limit", "Minimun degree for Y axis rotation"), min);
		}
		EditorGUILayout.Space();

		EditorGUI.indentLevel = 0;
		script.flexibleDistance = EditorGUILayout.Toggle(new GUIContent("Flexible Distance", "Camera moves closer if sight is blocked"), script.flexibleDistance);
		script.zoom = EditorGUILayout.Toggle(new GUIContent("Zoom", "Camera moves closer on input"), script.zoom);
		if (script.zoom)
		{
			EditorGUI.indentLevel = 1;
			script.minimumDistance = EditorGUILayout.FloatField(new GUIContent("Minimum Distance"), script.minimumDistance);
			script.maximumDistance = EditorGUILayout.FloatField(new GUIContent("Maximum Distance"), script.maximumDistance);
		}
	}
}
#endif