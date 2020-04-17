using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class BossBase : MonoBehaviour 
{
    public string name;
    public float blood;
    public float attack;

    public GameObject bossinfo;
    //boss 名称
    private string boss_name;
    //boss 血量
    private float boss_blood;
    //boss 攻击力
    private float boss_attack;
    //boss 状态 生死
    private bool boss_state;
    //boss 攻击状态
    private bool boss_attackstate;

    private bool boss_is_run = false;
    const float TIME = 2.0f;
    //自动时间间隔
    float timer = TIME;

    private NavMeshAgent bossnav;
    void Start()
    {
        InitBoss();
    }

    void Update()
    {
        //boss 当前为攻击状态
        if (boss_attackstate)
        {
            timer -= Time.deltaTime;
            //检测寻路
            if (CheckNavDistance(3f))
            {
                bossnav.GetComponent<NavMeshAgent>().enabled = false;
                if (timer <= 0)
                {
                    timer = TIME;
                    AttackManager.GetInstance().BossAttack(transform.gameObject, ConstantData.GetInstance().Player);
                }
            }
            else if (CheckNavDistance(6f))
            {
                bossnav.GetComponent<NavMeshAgent>().enabled = true;
                bossnav.destination = ConstantData.GetInstance().Player.transform.position;
                transform.GetComponent<BossAnimCon>().Run();
            }
            //检测跟随攻击距离
            CheckAttackDistance(6f);
        }
    }

    /// <summary>
    /// 初始化boss属性
    /// </summary>
    public void InitBoss()
    {
        //SetBossName(name);
        SetBossBlood(blood);
        SetBossAttack(attack);
        SetBossState(true);
        bossnav = transform.GetComponent<NavMeshAgent>();
    }
    public void SetBossName(string name)
    {
        Debug.Log("SetBossName：  " + name);
        this.boss_name = name;
        this.name = name;
    }
    public string GetBossName()
    {
        return boss_name;
    }

    public void SetBossBlood(float blood)
    {
        this.boss_blood = blood;
        this.blood = blood;
    }
    public float GetBossBlood()
    {
        return boss_blood;
    }

    public void SetBossAttack(float attack)
    {
        this.boss_attack = attack;
    }
    public float GetBossAttack()
    {
        return boss_attack;
    }
    public void SetBossState(bool state)
    {
        this.boss_state = state;
    }
    public bool GetBossState()
    {
        return boss_state;
    }

    public void SetBossAttackState(bool state)
    {
        this.boss_attackstate = state;
    }
    public bool GetBossAttackState()
    {
        return boss_attackstate;
    }
    public void Attack(GameObject gameobj)
    {
        gameobj.GetComponent<PlayerBase>().SetPlayerBlood(gameobj.GetComponent<PlayerBase>().GetPlayerBlood() - GetBossAttack());
    }

    public void SetBossRunState(bool state)
    {
        boss_is_run = state;
    }

    public bool GetBossIsRun()
    {
        return boss_is_run;
    }
    /// <summary>
    /// 检测boss是否在跟随攻击距离内
    /// </summary>
    /// <param name="dis"></param>
    public void CheckAttackDistance(float dis)
    {
        Debug.Log(name+" 检测距离，停止攻击");
        float distance = Vector3.Distance(ConstantData.GetInstance().Player.transform.position, ConstantData.GetInstance().currBoss.transform.position);
        //超出范围 停止攻击
        if (Mathf.Abs(distance) > dis)
        {
            boss_attackstate = false;
            transform.GetComponent<BossAnimCon>().Stand();
            bossnav.enabled = false;
        }
    }

    /// <summary>
    /// 检测是否在跟随范围内
    /// </summary>
    /// <param name="dis"></param>
    /// <returns></returns>
    public bool CheckNavDistance(float dis)
    {
        Debug.Log("检测是否跟随攻击");
        float distance = Vector3.Distance(ConstantData.GetInstance().Player.transform.position, ConstantData.GetInstance().currBoss.transform.position);
        return Mathf.Abs(distance) < dis;
    }
}
