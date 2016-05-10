using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 之前用Android控件写的2048,是通过数组的排列组合确定结果,现在的这个,是把每个元素都视作对象
/// </summary>
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
			Node[] tempN = { mNodes [i], mNodes [i + 4], mNodes [i + 8], mNodes [i + 12] };
			List<GameCube> cubeList = new List<GameCube> ();
			for (int j = 0; j < tempN.Length; j++) {
				if (tempN [j].mGameCube) {
					cubeList.Add (tempN [j].mGameCube);
				}
			}
			//检查元素,确定接下来的元素状态,确定Target也要在这里考虑,TODO
			int targetIndex=0;
			print("size="+cubeList.Count);
			for (int j = 0; j < cubeList.Count; j++) {
				if (j > 0 && cubeList [j - 1].mType == CubeType.F) {//typeD
					cubeList [j].mType = CubeType.D;
				} else if (j < (cubeList.Count - 1) && cubeList [j].mValue == cubeList [j + 1].mValue) {//typeF
					cubeList [j].mType = CubeType.F;
					targetIndex++;
				} else {//typeN
					targetIndex++;
				}
				print(targetIndex-1);
				cubeList [j].SetTarget (ref tempN [targetIndex-1], MoveDirection.Down);
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
