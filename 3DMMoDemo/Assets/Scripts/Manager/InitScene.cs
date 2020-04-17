using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitScene : MonoBehaviour {

	
	int BossCount = 6;
	// Use this for initialization
	void Start () {
		Debug.Log("InitScene Start");
		InitSceneState();
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void InitSceneState()
	{
		InitPlayer();
		InitBoss(BossCount);
	}

	/// <summary>
	/// 加载并初始化boss
	/// </summary>
	/// <param name="count"></param>
	private void InitBoss(int count)
	{
		for (int i = 0; i < count; i++)
		{
			//prefab
			GameObject boss = LoadBundle("Boss");
			//Vector3 newPos = RandomBossPosition(boss);
            int m = Random.Range(1, 6);
            int j = Random.Range(1, 6);
            Vector3 newPos = new Vector3(boss.transform.position.x + 10 * m, 0.008f, boss.transform.position.z + 10 * j);
            Quaternion rotation = boss.transform.rotation;
			GameObject newboss = Instantiate(boss,newPos,rotation);
			newboss.GetComponent<BossBase>().SetBossName("哥布林" + i);
			ConstantData.GetInstance().BossList.Add(newboss);

			UIController.GetInstance().AddBossItem(newboss);
		}
	}
	/// <summary>
	/// 加载并初始化player
	/// </summary>
	private void InitPlayer()
	{
		GameObject playerprefab = LoadBundle("Player");
		ConstantData.GetInstance().Player = Instantiate(playerprefab);

        GameObject playerhud = LoadBundle("PlayerHUD");
        ConstantData.GetInstance().PlayerHUD = Instantiate(playerhud);
		ConstantData.GetInstance().PlayerHUD.GetComponent<UIFollowTarget>().target = ConstantData.GetInstance().Player.transform.Find("player_ui_point").transform;
    }
	/// <summary>
	/// 生成随机位置
	/// </summary>
	/// <param name="boss"></param>
	/// <returns></returns>
	private Vector3 RandomBossPosition(GameObject boss)
	{
		int i = Random.Range(1, 4);
		int j = Random.Range(1, 4);
		Vector3 newPos = new Vector3(boss.transform.position.x + 10*i, 0.008f, boss.transform.position.z+10*j);
		return newPos;
	}

	private GameObject LoadBundle(string name)
	{
		return ConstantData.GetInstance().ab.LoadAsset<GameObject>(name) as GameObject;
	}
}
