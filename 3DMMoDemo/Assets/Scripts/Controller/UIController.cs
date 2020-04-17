using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class UIController : MonoBehaviour {

	public static UIController instance;
	public static UIController GetInstance()
	{
		return instance;
	}
	public GameObject BossListGrid;
	
	
	private bool player_auto_attack_state = false;
	private bool AttackIsCD = false;
	const float CDTime = 2.0f;
	float temptime = CDTime;
	float temptimeA = CDTime;
	float temptimeB = CDTime;
	// Use this for initialization
	void Start () {
		if(instance == null)
		{
			instance = this;
		}

		
	}
	
	// Update is called once per frame
	void Update () {
		temptime -= Time.deltaTime;
		temptimeA -= Time.deltaTime;
		temptimeB -= Time.deltaTime;
		//ChangePlayerCon();
		if (ConstantData.GetInstance().Player.GetComponent<PlayerBase>().GetPlayerAttackState())
		{
			CheckDisAndAttack(3f);
		}
		if(player_auto_attack_state)
		{
			Debug.Log("开启协程 自动攻击");
			StopAllCoroutines();
			player_auto_attack_state = false;
			StartCoroutine(PlayerAutoAttack());
		}
		//CheckDistanceToStopAttack(4.5f);
	}

	public void StopPlayerAutoAttack()
	{
		StopAllCoroutines();
	}
	/// <summary>
	/// 重新排序
	/// </summary>
	public void SortBossList()
	{
		BossListGrid.GetComponent<UIGrid>().Reposition();
	}

	public void AddBossItem(GameObject bossgameobj)
	{
		GameObject bossitem_prefab = LoadBundle("bossitem");
		bossitem_prefab.transform.localScale = Vector3.one;
		//实例化gameobject 为bosslist子物体
		GameObject bossitem = Instantiate(bossitem_prefab, BossListGrid.transform);
		bossitem.GetComponent<BossItemBase>().BossGameObject = bossgameobj;
		bossgameobj.GetComponent<BossBase>().bossinfo = bossitem;
		bossitem.GetComponent<UIButton>().onClick.Add(new EventDelegate(()=> 
		{
			Debug.Log("寻路自动攻击");
			//ConstantData.GetInstance().Player.transform.LookAt(bossgameobj.transform);
			PlayerAutoAttack(bossgameobj); 
		}));
		SortBossList();
	}

	public void PlayerAutoAttack(GameObject bossgamobj)
	{
		ConstantData.GetInstance().Player.GetComponent<NavMeshAgent>().enabled = true;
		ConstantData.GetInstance().Player.GetComponent<NavMeshAgent>().destination = bossgamobj.transform.position;

		ConstantData.GetInstance().Player.transform.LookAt(bossgamobj.transform);
		bossgamobj.transform.LookAt(ConstantData.GetInstance().Player.transform);
		ConstantData.GetInstance().Player.GetComponent<PlayerBase>().SetPlayerAttackState(true);
		//设置bossUI信息
		ConstantData.GetInstance().BossInfo.SetActive(true);
		ConstantData.GetInstance().BossInfo.GetComponentInChildren<UISlider>().value = bossgamobj.GetComponent<BossBase>().GetBossBlood() / 100f;
		ConstantData.GetInstance().BossInfo.GetComponentInChildren<UILabel>().text = bossgamobj.GetComponent<BossBase>().GetBossName();
		ConstantData.GetInstance().currBoss = bossgamobj;
	}

	public void CheckDisAndAttack(float dis)
	{
		if(ConstantData.GetInstance().Player.GetComponent<PlayerBase>().GetPlayerAttackState())
		{
			
			float distance = Vector3.Distance(ConstantData.GetInstance().Player.transform.position, ConstantData.GetInstance().currBoss.transform.position);
			ConstantData.GetInstance().Player.GetComponent<PlayerAnimCon>().Run();
			if (Mathf.Abs(distance) < dis)
            {
				ConstantData.GetInstance().Player.GetComponent<PlayerBase>().SetPlayerAttackState(false);
				ConstantData.GetInstance().Player.GetComponent<PlayerBase>().SetPlayerAutoAttackState(true);
				player_auto_attack_state = true;
				ConstantData.GetInstance().Player.GetComponent<NavMeshAgent>().enabled = false;
			}
        }
	}
	/// <summary>
	/// 主角npc自动攻击协程
	/// </summary>
	/// <returns></returns>
	private IEnumerator PlayerAutoAttack()
	{
		while(ConstantData.GetInstance().Player.GetComponent<PlayerBase>().GetPlayerAutoAttackState())
		{
			Debug.Log("Player 开始自动攻击");
			ConstantData.GetInstance().Player.GetComponent<PlayerAnimCon>().Attack();
			ConstantData.GetInstance().Player.GetComponent<PlayerAnimCon>().CheckAttack();
            yield return new WaitForSeconds(2.0f);
			Debug.Log("Player 自动攻击");  
        }
	}

    public void CheckDistanceToStopAttack(float dis)
    {
		if(ConstantData.GetInstance().currBoss != null)
		{
            float distance = Vector3.Distance(ConstantData.GetInstance().Player.transform.position, ConstantData.GetInstance().currBoss.transform.position);
            if (Mathf.Abs(distance) > dis)
            {
				AttackManager.GetInstance().StopAttakc();
            }
        }
    }
	/// <summary>
	/// 设置键盘移动
	/// </summary>
	public void ChangePlayerCon()
	{
		if(ConstantData.GetInstance().EasyTouchConToggle.GetComponent<UIToggle>().value)
		{
			ConstantData.GetInstance().Player.GetComponent<PlayerMove>().enabled = true;

		}
		else
		{
			ConstantData.GetInstance().Player.GetComponent<PlayerMove>().enabled = false;
		}
	}
	/// <summary>
	/// 设置Bloom特效
	/// </summary>
	public void ChangeBloomEffect()
	{
		if(ConstantData.GetInstance().BlommEffectToggle.GetComponent<UIToggle>().value)
		{
			ConstantData.GetInstance().BloomEffect.SetActive(true);
		}
		else
		{
			ConstantData.GetInstance().BloomEffect.SetActive(false);
		}
	}
	/// <summary>
	/// 添加boss
	/// </summary>
	public void AddBoss()
	{
        //prefab
        GameObject boss = LoadBundle("Boss");
        //Vector3 newPos = RandomBossPosition(boss);
        int m = Random.Range(1, 6);
        int j = Random.Range(1, 6);
        Vector3 newPos = new Vector3(boss.transform.position.x + 10 * m, 0.008f, boss.transform.position.z + 10 * j);
        Quaternion rotation = boss.transform.rotation;
        GameObject newboss = Instantiate(boss, newPos, rotation);
        newboss.GetComponent<BossBase>().SetBossName("哥布林");
        ConstantData.GetInstance().BossList.Add(newboss);
		
		UIController.GetInstance().AddBossItem(newboss);	
	}

	/// <summary>
	/// 复活玩家
	/// </summary>
	public void ResetPlayer()
	{
		ConstantData.GetInstance().Player.gameObject.SetActive(true);
		//ConstantData.GetInstance().Player.transform.position = new Vector3(ConstantData.GetInstance().Player.transform.position.x,0.5f, ConstantData.GetInstance().Player.transform.position.z);
		ConstantData.GetInstance().Player.GetComponent<PlayerBase>().InitPlayer();
		ConstantData.GetInstance().Player.GetComponent<PlayerAnimCon>().Stand();
		
    }

	/// <summary>
	/// 攻击键
	/// </summary>
    public void OnAttackClick()
    {
        if (!ConstantData.GetInstance().Player.GetComponent<PlayerBase>().GetPlayerAttackState() && temptime < 0)
        {
			ConstantData.GetInstance().Player.GetComponent<PlayerAnimCon>().Attack();
			ConstantData.GetInstance().Player.GetComponent<PlayerAnimCon>().CheckAttack();
			temptime = CDTime;
		}
		else
		{
			Debug.Log("技能冷却中");
		}
    }

	public void OnAttackAClick()
	{
		if (!ConstantData.GetInstance().Player.GetComponent<PlayerBase>().GetPlayerAttackState() && temptimeA < 0)
		{
			ConstantData.GetInstance().Player.GetComponent<PlayerAnimCon>().AttackA();
			ConstantData.GetInstance().Player.GetComponent<PlayerAnimCon>().CheckAttack();
			temptimeA = CDTime;
		}
		else
		{
			Debug.Log("技能冷却中");
		}
	}

	public void OnAttackBClick()
	{
		if (!ConstantData.GetInstance().Player.GetComponent<PlayerBase>().GetPlayerAttackState() && temptimeB < 0)
		{
			ConstantData.GetInstance().Player.GetComponent<PlayerAnimCon>().AttackB();
			ConstantData.GetInstance().Player.GetComponent<PlayerAnimCon>().CheckAttack();
			temptimeB = CDTime;
		}
		else
		{
			Debug.Log("技能冷却中");
		}
	}
	private GameObject LoadBundle(string name)
    {
        return ConstantData.GetInstance().ab.LoadAsset<GameObject>(name) as GameObject;
    }
}
