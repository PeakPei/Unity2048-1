using UnityEngine;
using System.Collections;

public class GameCube : MonoBehaviour
{
	public int mValue;
	public Node mCurrentNode;
	public Node mTargetNode;
	public CubeType mType;
	public Sprite[] mSprites;
	private SpriteRenderer mRender;
	[HideInInspector]
	public bool isMoving;//标记GameCube是否在移动

	// Use this for initialization
	void Start ()
	{
		mRender = GetComponent<SpriteRenderer> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		//这里不包含状态改变阶段,状态改变由全局角色控制,这里只处理状态改变之后,也就是mTargetNode改变之后的逻辑
		if (mTargetNode) {
			if (mCurrentNode != mTargetNode) {//移动位置阶段
				isMoving=true;
				GameManager.GetInstance().hasCubeMoving=true;
				transform.position = Vector3.MoveTowards (transform.position, mTargetNode.transform.position, Time.deltaTime * 10);
				if (Vector3.Distance (transform.position, mTargetNode.transform.position) < 0.1f) {
					mCurrentNode = mTargetNode;
					isMoving=false;
				}
			} else {//移动完成阶段
				switch (mType) {
				case CubeType.N:
					break;
				case CubeType.F:
					GameManager.GetInstance ().score += Mathf.Log (mValue, 2);
					;//每次融合增加奖励分,暂时只用了对数,没有放大
					mValue *= 2;
					switch (mValue) {
					case 2:
						mRender.sprite = mSprites [0];
						break;
					case 4:
						mRender.sprite = mSprites [1];
						break;
					case 8:
						mRender.sprite = mSprites [2];
						break;
					case 16:
						mRender.sprite = mSprites [3];
						break;
					case 32:
						mRender.sprite = mSprites [4];
						break;
					case 64:
						mRender.sprite = mSprites [5];
						break;
					case 128:
						mRender.sprite = mSprites [6];
						break;
					case 256:
						mRender.sprite = mSprites [7];
						break;
					case 512:
						mRender.sprite = mSprites [8];
						break;
					case 1024:
						mRender.sprite = mSprites [9];
						break;
					case 2048:
						mRender.sprite = mSprites [10];
						break;
					case 4096:
						mRender.sprite = mSprites [11];
						break;
					case 8192:
						mRender.sprite = mSprites [12];
						break;
					}
					mType = CubeType.N;
					break;
				case CubeType.D:
					GameObject.Destroy (this.gameObject);
					break;
				}
			}
		}
	}

	public void SetTarget (ref Node target, MoveDirection md)
	{
		mCurrentNode.mGameCube = null;
		mTargetNode = target;
		if (mType != CubeType.D) {//删除的元素不需要成为任何Node的Cube
			target.mGameCube = this;
		}
	}
}
