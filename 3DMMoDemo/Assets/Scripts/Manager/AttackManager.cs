using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackManager : MonoBehaviour {

	public static AttackManager instance;
	public static AttackManager GetInstance()
	{
		return instance;
	}
	// Use this for initialization
	void Start () 
	{
		if(instance == null)
		{
			instance = this;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Space))
		{
			Debug.Log("停止攻击");
			StopAttakc();
		}
	}

	/// <summary>
	/// 停止自动攻击
	/// </summary>
	public void StopAttakc()
	{
		StopAllCoroutines();
		UIController.GetInstance().StopPlayerAutoAttack();
	}
	/// <summary>
	/// boss 自动攻击
	/// </summary>
	/// <param name="boss"></param>
	/// <param name="player"></param>
	/// <param name="time"></param>
	public void BossAutoAttack(GameObject boss,GameObject player,float time)
	{
		boss.GetComponent<BossBase>().SetBossAttackState(true);
		//StopAllCoroutines();
		StartCoroutine(AttackTime(boss, player, time));
	}
	/// <summary>
	/// 自动攻击协程
	/// </summary>
	/// <param name="boss"></param>
	/// <param name="player"></param>
	/// <param name="time"></param>
	/// <returns></returns>
	private IEnumerator AttackTime(GameObject boss, GameObject player, float time)
	{
		Debug.Log("Boss Auto Attack"+ boss.GetComponent<BossBase>().GetBossAttackState());
		while(boss.GetComponent<BossBase>().GetBossAttackState())
		{
			yield return new WaitForSeconds(time);
			BossAttack(boss, player);
		}
	}
	/// <summary>
	/// boss 攻击
	/// </summary>
	/// <param name="boss"></param>
	/// <param name="player"></param>
	public void BossAttack(GameObject boss,GameObject player)
	{
		//boss 攻击状态 且 player 存活
		if(boss.GetComponent<BossBase>().GetBossAttackState() && player.GetComponent<PlayerBase>().GetPlayerState())
		{
			//boss.GetComponent<BossBase>().SetBossAttackState(true);
			boss.transform.LookAt(player.transform);
			boss.GetComponent<BossAnimCon>().Attack();
			float temp = player.GetComponent<PlayerBase>().GetPlayerBlood() - boss.GetComponent<BossBase>().GetBossAttack();
			if (temp > 0)
			{
				player.GetComponent<PlayerBase>().SetPlayerBlood(player.GetComponent<PlayerBase>().GetPlayerBlood() - boss.GetComponent<BossBase>().GetBossAttack());
				ConstantData.GetInstance().PlayerHUD.GetComponent<HUDText>().Add(-boss.GetComponent<BossBase>().GetBossAttack(), Color.red, 0.5f);
				Debug.Log(player.GetComponent<PlayerBase>().GetPlayerName() + " 血量" + player.GetComponent<PlayerBase>().GetPlayerBlood());
				Debug.Log("血量减少" + boss.GetComponent<BossBase>().GetBossAttack());
			}
			else
			{
				player.GetComponent<PlayerBase>().SetPlayerBlood(0);
				player.GetComponent<PlayerBase>().SetPlayerState(false);
				player.GetComponent<PlayerBase>().SetPlayerAttackState(false);
				player.GetComponent<PlayerBase>().SetPlayerAutoAttackState(false);
				boss.GetComponent<BossBase>().SetBossAttackState(false);
				StopAllCoroutines();
				boss.GetComponent<BossAnimCon>().Stand();
				player.GetComponent<PlayerAnimCon>().Dead();
				PublicFuncs.GetInstance().WaitTimeDoFun(0.5f, () =>
				{
					player.SetActive(false);
					
				});
			}

			ConstantData.GetInstance().PlayerInfo.GetComponentInChildren<UISlider>().value = player.GetComponent<PlayerBase>().GetPlayerBlood() / 100f;
		}
	}
	/// <summary>
	/// player 攻击
	/// </summary>
	/// <param name="player"></param>
	/// <param name="boss"></param>
	public void PlayerAttack(GameObject player,GameObject bosstemp)
	{
		ShowBossInfo(player,bosstemp);
		//显示boss 信息
		ConstantData.GetInstance().BossInfo.SetActive(true);
		//bosstemp.GetComponent<BossBase>().Attack(player.gameObject);
		float temp = bosstemp.gameObject.GetComponent<BossBase>().GetBossBlood() - player.GetComponent<PlayerBase>().GetPlayerAttack();
		//boss 掉血
        if (temp > 0)
        {
            bosstemp.GetComponent<BossBase>().SetBossBlood(bosstemp.GetComponent<BossBase>().GetBossBlood() - player.GetComponent<PlayerBase>().GetPlayerAttack());
			PublicFuncs.GetInstance().WaitTimeDoFun(0.5f, ()=> { ShowBossInfo(player, bosstemp); });
            GameObject bosshud = LoadBundle("BossHUD");
            ConstantData.GetInstance().BossHUD = Instantiate(bosshud);
            ConstantData.GetInstance().BossHUD.GetComponent<UIFollowTarget>().target = bosstemp.transform.Find("boss_ui_point").transform;
			//ConstantData.GetInstance().BossHUD.GetComponent<UIFollowTarget>().target = bosstemp.transform;
			ConstantData.GetInstance().BossHUD.GetComponent<HUDText>().Add(-player.GetComponent<PlayerBase>().GetPlayerAttack(), Color.red, 0.5f);
			Debug.Log(bosstemp.GetComponent<BossBase>().GetBossName() + " 血量" + bosstemp.GetComponent<BossBase>().GetBossBlood());
            Debug.Log("血量减少" + player.GetComponent<PlayerBase>().GetPlayerAttack());
        }
		//boss 死亡
        else if (bosstemp.GetComponent<BossBase>().GetBossState())
        {
			
			bosstemp.GetComponent<BossBase>().SetBossBlood(0);
            player.GetComponent<PlayerBase>().SetPlayerAttackState(false);
            player.GetComponent<PlayerBase>().SetPlayerAutoAttackState(false);
            PublicFuncs.GetInstance().WaitTimeDoFun(0.8f, () =>
            {
				ConstantData.GetInstance().BossList.Remove(bosstemp);
                bosstemp.GetComponent<BossAnimCon>().Dead();
				StopAllCoroutines();
				//隐藏死亡的boss
				PublicFuncs.GetInstance().WaitTimeDoFun(0.5f, () =>
				{
					bosstemp.SetActive(false);
					ConstantData.GetInstance().BossInfo.SetActive(false);
					ConstantData.GetInstance().BossHUD = null;
					bosstemp.GetComponent<BossBase>().SetBossState(false);
					//update boss list
                    bosstemp.GetComponent<BossBase>().bossinfo.SetActive(false);
                    UIController.GetInstance().SortBossList();
                });
				
            });
        }
    }

	public void ShowBossInfo(GameObject player,GameObject boss)
	{
		GameObject bossinfo = ConstantData.GetInstance().BossInfo;
		bossinfo.GetComponentInChildren<UISlider>().value = boss.GetComponent<BossBase>().GetBossBlood() / 100f;
		bossinfo.GetComponentInChildren<UILabel>().text = boss.GetComponent<BossBase>().GetBossName();
	}

    private GameObject LoadBundle(string name)
    {
        return ConstantData.GetInstance().ab.LoadAsset<GameObject>(name) as GameObject;
    }

}
