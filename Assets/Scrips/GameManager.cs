using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{

	public Node[] mNodes;

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public void MoveDown ()
	{
		for (int i = 0; i < 4; i++) {
			Node[] tempN={mNodes[i],mNodes[i+4],mNodes[i+8],mNodes[i+12]};
			for (int j = 0; j < tempN.Length; j++) {
//				print("llele");
				if (tempN[j].mGameCube) {
					tempN[j].mGameCube.SetTarget(ref mNodes[i],MoveDirection.Down);
				}
			}
		}
	}




	void OnGUI ()
	{
		if (GUILayout.Button ("UP")) {
			
		}
		if (GUILayout.Button ("LEFT")) {

		}
		if (GUILayout.Button ("DOWN")) {
			MoveDown ();
		}
		if (GUILayout.Button ("RIGHT")) {

		}
	}
}
