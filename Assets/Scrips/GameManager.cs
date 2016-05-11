using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 之前用Android控件写的2048,是通过数组的排列组合确定结果,现在的这个,是把每个元素都视作对象
/// </summary>
public class GameManager : MonoBehaviour
{

	public Node[] mNodes;

	[HideInInspector ()]
	public float score;
	private static GameManager instance;

	void Awake ()
	{
		instance = this;
	}

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public static GameManager GetInstance ()
	{
		return instance;
	}

	public void MoveDown (MoveDirection direction)
	{
		for (int i = 0; i < 4; i++) {
			Node[] tempN = new Node[]{ };
			//根据输入的滑动方向决定获取元素的顺序
			switch (direction) {
			case MoveDirection.Up://每列从上到下
				tempN = new Node[]{ mNodes [i + 12], mNodes [i + 8], mNodes [i + 4], mNodes [i] };
				break;
			case MoveDirection.Left://每行从左到右
				tempN = new Node[]{ mNodes [i * 4], mNodes [i * 4 + 1], mNodes [i * 4 + 2], mNodes [i * 4 + 3] };
				break;
			case MoveDirection.Down://每列从下到上
				tempN = new Node[]{ mNodes [i], mNodes [i + 4], mNodes [i + 8], mNodes [i + 12] };
				break;
			case MoveDirection.Right://每行从右到左
				tempN = new Node[]{ mNodes [i * 4 + 3], mNodes [i * 4 + 2], mNodes [i * 4 + 1], mNodes [i * 4] };
				break;
			}
			List<GameCube> cubeList = new List<GameCube> ();
			for (int j = 0; j < tempN.Length; j++) {
				if (tempN [j].mGameCube) {
					cubeList.Add (tempN [j].mGameCube);
				}
			}
			//检查元素,确定接下来的元素状态,确定Target也要在这里考虑
			int targetIndex = 0;
			print ("size=" + cubeList.Count);
			for (int j = 0; j < cubeList.Count; j++) {
				if (j > 0 && cubeList [j - 1].mType == CubeType.F) {//typeD,删除元素
					cubeList [j].mType = CubeType.D;
					//该元素将被删除,所以不需要目标位递增
				} else if (j < (cubeList.Count - 1) && cubeList [j].mValue == cubeList [j + 1].mValue) {//typeF,融合元素
					cubeList [j].mType = CubeType.F;
					targetIndex++;
				} else {//typeN,酱油元素
					targetIndex++;
				}
				cubeList [j].SetTarget (ref tempN [targetIndex - 1], MoveDirection.Down);
			}
		}
	}

	void OnGUI ()
	{
		//可以在控制输入的地方加一个invoke限制连续输入,TODO
		if (GUILayout.Button ("UP")) {
			MoveDown (MoveDirection.Up);
		}
		if (GUILayout.Button ("LEFT")) {
			MoveDown (MoveDirection.Left);
		}
		if (GUILayout.Button ("DOWN")) {
			MoveDown (MoveDirection.Down);
		}
		if (GUILayout.Button ("RIGHT")) {
			MoveDown (MoveDirection.Right);
		}
		if (GUILayout.Button ("Test")) {
			float a = Utils.Log (8, 2);
			print (a.ToString ());
		}
		GUILayout.Label (score.ToString ());
	}
}
