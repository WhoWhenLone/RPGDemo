using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantData : MonoBehaviour {

	public static ConstantData instance;
	public static ConstantData GetInstance()
	{
		return instance;
	}
	// Use this for initialization
	void Start () {
		if(instance == null)
		{
			instance = this;
		}
		ab = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/AssetsBundle/npcbundle");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	#region 公共数据
	//boss 列表
	public List<GameObject> BossList = new List<GameObject>();
	//玩家
	public GameObject Player;
	//当前攻击boss
	public GameObject currBoss;
	#endregion

	#region UI资源管理
	public GameObject PlayerInfo;
	public GameObject BossInfo;
	public GameObject PlayerHUD;
	public GameObject BossHUD;
	public GameObject ScrollBar;
	public GameObject EasyTouchConToggle;
	public GameObject BlommEffectToggle;
	#endregion
	public AssetBundle ab;

	public GameObject BloomEffect;
}
