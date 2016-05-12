using UnityEngine;
using System.Collections.Generic;
using System.Collections;

/// <summary>
/// 之前用Android控件写的2048,是通过数组的排列组合确定结果,现在的这个,是把每个元素都视作对象
/// </summary>
public class GameManager : MonoBehaviour
{
	//对应盘面上的每个点
	public Node[] mNodes;
	public GameCube mCubePrefab;
	private static GameManager instance;
	//标记盘面移动状态
	[HideInInspector]
	public bool hasCubeMoving;
	[HideInInspector ()]
	public float score;
	//游戏状态
	[HideInInspector]
	private GameStatus status;

	void Awake ()
	{
		instance = this;
	}

	// Use this for initialization
	void Start ()
	{
		GameStart ();
	}
	
	// Update is called once per frame
	void Update ()
	{

	}

	private void GameStart ()
	{
		status = GameStatus.Playing;
		StartCoroutine (CoroutineUpdate ());
	}

	private void GameOver ()
	{
		status = GameStatus.End;
		StopCoroutine (CoroutineUpdate ());
	}

	/// <summary>
	/// 伪Update,检查游戏状态,这个不需要频率太高
	/// </summary>
	/// <returns>The update.</returns>
	private IEnumerator CoroutineUpdate ()
	{
		while (true) {
			yield return new WaitForSeconds (0.2f);
			//这里的检查应该分为两个优先级,这样每次只跑一个轮子
			//首先检查盘面是否由运动状态恢复到静止状态,方法是遍历每个Node上的元素的运动状态.
			if (hasCubeMoving) {
				Debug.Log ("Moving");
				for (int i = 0; i < mNodes.Length; i++) {
					if (mNodes [i].mGameCube) {
						if (mNodes [i].mGameCube.isMoving) {
							break;
						} else if (i == (mNodes.Length - 1)) {
							//说明到了最后一个
							hasCubeMoving = false;
							//添加新cube
							AddCube ();
						}
					} else if (i == (mNodes.Length - 1)) {
						//说明到了最后一个
						hasCubeMoving = false;
						//添加新cube
						AddCube ();
					}
				}
			}
			//其次是检查盘面状态是否还能移动,如不能再动则游戏结束,方法是检查每个Node上的Cube的Right或Top是否有空格子或是值相等的Cube
			else {
				Debug.Log ("Stop");
				for (int i = 0; i < mNodes.Length; i++) {
					if (!mNodes [i].mGameCube) {//当前位置为空位置
						break;
					} else if (mNodes [i].mUp && (!(mNodes [i].mUp.mGameCube))) {
						break;
					} else if (mNodes [i].mRight && (!(mNodes [i].mRight.mGameCube))) {
						break;
					} else if (mNodes [i].mUp && (mNodes [i].mGameCube.mValue == mNodes [i].mUp.mGameCube.mValue)) {
						break;
					} else if (mNodes [i].mRight && (mNodes [i].mGameCube.mValue == mNodes [i].mRight.mGameCube.mValue)) {
						break;
					} else {
						if (i == (mNodes.Length - 1)) {
							//说明到了最后一个
							Debug.Log ("GameOver!!!");
						}
					}
				}
			}
		}
	}

	/// <summary>
	/// 在盘面空位置上随机添加GameCube
	/// </summary>
	private void AddCube ()
	{
		//先获取到所有空位置
		List<Node> tempList = new List<Node> ();
		for (int i = 0; i < mNodes.Length; i++) {
			if (!mNodes [i].mGameCube) {
				tempList.Add (mNodes [i]);
			}
		}
		int pos = Random.Range (0, (tempList.Count - 1));
		Debug.Log ("positon=" + tempList [pos].mPosX + "-" + tempList [pos].mPosY + ",index=" + pos);
		GameCube cube = Instantiate (mCubePrefab, tempList [pos].transform.position, Quaternion.identity) as GameCube;
		//乱入数,2或者4
		int num=Random.value>0.4?2:4;
		if (num==2) {
			cube.mValue = 2;
			cube.GetComponent<SpriteRenderer> ().sprite = cube.mSprites [0];
		}else{
			cube.mValue = 4;
			cube.GetComponent<SpriteRenderer> ().sprite = cube.mSprites [1];
		}
		cube.mCurrentNode = tempList [pos];
		tempList [pos].mGameCube = cube;
	}

	public static GameManager GetInstance ()
	{
		return instance;
	}

	private void MoveBoard (MoveDirection direction)
	{
		if (status == GameStatus.Playing) {
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
				int targetIndex = 0;//标记移动的目标位置
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
	}

	void OnGUI ()
	{
		//可以在控制输入的地方加一个invoke限制连续输入,TODO
		if (GUILayout.Button ("UP")) {
			MoveBoard (MoveDirection.Up);
		}
		if (GUILayout.Button ("LEFT")) {
			MoveBoard (MoveDirection.Left);
		}
		if (GUILayout.Button ("DOWN")) {
			MoveBoard (MoveDirection.Down);
		}
		if (GUILayout.Button ("RIGHT")) {
			MoveBoard (MoveDirection.Right);
		}
		if (GUILayout.Button ("Test")) {
			AddCube ();
		}
		GUILayout.Label (score.ToString ());
	}
}
