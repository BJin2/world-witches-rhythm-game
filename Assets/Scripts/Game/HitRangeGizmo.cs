using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

#if UNITY_EDITOR
public class HitRangeGizmo : MonoBehaviour
{
	//Hard coded width and height for this project
	private const float x = 12;
	private const float y = 1;

	[DrawGizmo(GizmoType.Selected | GizmoType.NotInSelectionHierarchy)]
#pragma warning disable IDE0051 // Remove unused private members
#pragma warning disable IDE0060 // Remove unused parameter
	static void DrawRange(HitRange range, GizmoType gizmoType)
#pragma warning restore IDE0060 // Remove unused parameter
#pragma warning restore IDE0051 // Remove unused private members
	{
		for (int i = 0; i < range.count; i++)
		{
			Gizmos.color = range.colors[i];
			Gizmos.DrawWireCube(range.transform.position, new Vector3(x, y, range.ranges[i]));
		}
	}

	[DrawGizmo(GizmoType.NotInSelectionHierarchy | GizmoType.Pickable)]
#pragma warning disable IDE0051 // Remove unused private members
#pragma warning disable IDE0060 // Remove unused parameter
	static void DrawIcon(HitRange range, GizmoType gizmoType)
#pragma warning restore IDE0060 // Remove unused parameter
#pragma warning restore IDE0051 // Remove unused private members
	{
		string path = "Icons\\HitIcon.png";
		Gizmos.DrawIcon(range.transform.position, path, false);
	}
}

[CustomEditor(typeof(HitRange))]
public class HitRangeEditor : Editor
{
#region Inspector
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		HitRange range = (HitRange)target;

		EditorGUI.BeginChangeCheck();
		int newCount = EditorGUILayout.IntField(new GUIContent("Range Step"), range.count);
		if (EditorGUI.EndChangeCheck())
		{
			range.CountChanged(newCount);
		}
		EditorGUILayout.Space();

		int count = 0;
		for (int i = range.count-1; i >= 0; i--)
		{
			EditorGUI.indentLevel = 0;
			EditorGUILayout.LabelField("Range " + count.ToString());
			EditorGUI.indentLevel = 1;
			range.ranges[i] = EditorGUILayout.FloatField(new GUIContent("Range"), range.ranges[i]);
			range.colors[i] = EditorGUILayout.ColorField(new GUIContent("Color"), range.colors[i]);
			count++;
		}
	}
#endregion

#region Handle
	private void OnSceneGUI()
	{
		HitRange hit = (HitRange)target;

		for (int i = 0; i < hit.count; i++)
		{
			hit.ranges[i] = ChangeRange(hit.transform.position, hit.ranges[i], hit.colors[i]);
		}
		hit.LimitRange();
	}

	//Symmetry
	private float ChangeRange(Vector3 origin, float range, Color color)
	{
		Handles.color = color;
		float handleSize = 0.5f;
		float handleHalf = handleSize / 2.0f;

		//Forward
		EditorGUI.BeginChangeCheck();
		Vector3 handlePosition = new Vector3(origin.x, origin.y, origin.z + (range / 2.0f) + handleHalf);
		Vector3 newRange = Handles.Slider(handlePosition, Vector3.forward, handleSize, Handles.ConeHandleCap, 0.1f);
		if (EditorGUI.EndChangeCheck())
		{
			
			HitRange hit = (HitRange)target;
			Undo.RecordObject(hit, "Changed Range(Forward)");

			range = (newRange.z - origin.z) + (range / 2.0f) - handleHalf;

			//Debug.Log("H : " + handlePosition.z);
			//Debug.Log("N : " + newRange.z);
			//Debug.Log("R : " + range);
		}

		//backward
		EditorGUI.BeginChangeCheck();
		Vector3 handlePosition_back = new Vector3(origin.x, origin.y, origin.z - (range / 2.0f) - handleHalf);
		Vector3 newRange_back = Handles.Slider(handlePosition_back, Vector3.back, handleSize, Handles.ConeHandleCap, 0.1f);
		if (EditorGUI.EndChangeCheck())
		{
			HitRange hit = (HitRange)target;
			Undo.RecordObject(hit, "Changed Range(Backward)");

			range = ((newRange_back.z + handleHalf) * -1) + origin.z + (range / 2.0f);

			//Debug.Log("H : " + handlePosition_back.z);
			//Debug.Log("N : " + newRange_back.z);
			//Debug.Log("R : " + range);
		}

		return range;
	}
#endregion
}
#endif