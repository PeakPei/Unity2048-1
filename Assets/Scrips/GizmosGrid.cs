using UnityEngine;
using System.Collections;

public class GizmosGrid : MonoBehaviour
{
	//	public Color color;

	void OnDrawGizmos ()
	{
		DrawGrid ();
	}

	private void DrawGrid ()
	{
		Gizmos.color = Color.yellow;
		for (int i = 0; i < 5; i++) {
			Gizmos.DrawLine (new Vector3 (0, i, 0), new Vector3 (4, i, 0));
		}
		for (int i = 0; i < 5; i++) {
			Gizmos.DrawLine (new Vector3 (i, 0, 0), new Vector3 (i, 4, 0));
		}

	}


}
