using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

#if UNITY_EDITOR
public class HitRangeGizmo : MonoBehaviour
{
	private const float x = 12;
	private const float y = 1;

	[DrawGizmo(GizmoType.Selected | GizmoType.NotInSelectionHierarchy | GizmoType.Pickable)]
	static void DrawRange(HitRange range, GizmoType gizmoType)
	{
		for (int i = 0; i < 4; i++)
		{
			Gizmos.color = range.colors[i];
			Gizmos.DrawWireCube(range.transform.position, new Vector3(x, y, range.ranges[i]));
		}
	}
}

[CustomEditor(typeof(HitRange))]
public class HitRangeEditor : Editor
{
	private void OnSceneGUI()
	{
		HitRange hit = (HitRange)target;

		//EditorGUI.BeginChangeCheck();
		//Vector3 newPos = Handles.Slider(hit.transform.position, Vector3.back, 0.5f, Handles.ConeHandleCap, 0.1f);
		//if (EditorGUI.EndChangeCheck())
		//{
		//	Undo.RecordObject(hit, "Pos");
		//	hit.transform.position = newPos;
		//}

		for (int i = 0; i < 4; i++)
		{
			ChangeRange(hit.transform.position, ref hit.ranges[i], hit.colors[i]);
		}
	}

	private void ChangeRange(Vector3 origin, ref float range, Color color)
	{
		Handles.color = color;

		//Forward
		EditorGUI.BeginChangeCheck();
		Vector3 handlePosition = new Vector3(origin.x, origin.y, origin.z + (range / 2.0f) + 0.25f);
		Vector3 newRange = Handles.Slider(handlePosition, Vector3.forward, 0.5f, Handles.ConeHandleCap, 0.1f);
		if (EditorGUI.EndChangeCheck())
		{
			
			HitRange hit = (HitRange)target;
			Undo.RecordObject(hit, "Changed Range(Forward)");

			range = (newRange.z - origin.z) + (range / 2.0f) - 0.25f;

			//Debug.Log("H : " + handlePosition.z);
			//Debug.Log("N : " + newRange.z);
			//Debug.Log("R : " + range);

			hit.LimitRange();
		}

		//backward
		EditorGUI.BeginChangeCheck();
		Vector3 handlePosition_back = new Vector3(origin.x, origin.y, origin.z - (range / 2.0f) - 0.25f);
		Vector3 newRange_back = Handles.Slider(handlePosition_back, Vector3.back, 0.5f, Handles.ConeHandleCap, 0.1f);
		if (EditorGUI.EndChangeCheck())
		{
			HitRange hit = (HitRange)target;
			Undo.RecordObject(hit, "Changed Range(Backward)");

			range = ((newRange_back.z + 0.25f) * -1) + origin.z + (range / 2.0f);

			//Debug.Log("H : " + handlePosition_back.z);
			//Debug.Log("N : " + newRange_back.z);
			//Debug.Log("R : " + range);

			hit.LimitRange();
		}
	}
}
#endif