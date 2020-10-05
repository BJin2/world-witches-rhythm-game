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
	static void DrawPerfectZone(HitRange range, GizmoType gizmoType)
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireCube(range.transform.position, new Vector3(x, y, 15));

		Gizmos.color = Color.yellow;
		Gizmos.DrawWireCube(range.transform.position, new Vector3(x, y, 10));

		Gizmos.color = Color.blue;
		Gizmos.DrawWireCube(range.transform.position, new Vector3(x, y, 4));

		Gizmos.color = Color.green;
		Gizmos.DrawWireCube(range.transform.position, new Vector3(x, y, 1));
	}
}
#endif