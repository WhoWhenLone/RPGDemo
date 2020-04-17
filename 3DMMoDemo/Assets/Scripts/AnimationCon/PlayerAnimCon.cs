using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.AI;
public class PlayerAnimCon : MonoBehaviour
{
    public AnimationClip[] clips;
    public GameObject AttackWQ;
    public GameObject NormalWQ;
    private Animation animation;
    private NavMeshAgent navmesh;
    // Use this for initialization
    void Start()
    {
        animation = transform.GetComponentInChildren<Animation>();
        navmesh = transform.GetComponent<NavMeshAgent>();
        IsAttackWQState(0);
        Stand();
    }
    /// <summary>
    /// 切换攻击状态
    /// </summary>
    /// <param name="state"></param>
    public void IsAttackWQState(int state)
    {
        if(state == 0)
        {
            AttackWQ.SetActive(false);
            NormalWQ.SetActive(true);
        }
        else
        {
            AttackWQ.SetActive(true);
            NormalWQ.SetActive(false);
        }

    }
    // Update is called once per frame
    void Update()
    {
        if(!transform.gameObject.GetComponent<PlayerBase>().GetPlayerState())
        {
            return;
        }
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

    }

    public void CheckAttack()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 2.5f, LayerMask.GetMask("Boss"));
        Debug.Log("Len"+colliders.Length);
        if (colliders.Length > 0)
        {
            for (int i = 0; i < colliders.Length; i++)
            {
                GameObject bosstemp = colliders[i].gameObject;
                //ConstantData.GetInstance().currBoss = bosstemp;
                //触发主角攻击
               // AttackManager.GetInstance().PlayerAttack(transform.gameObject, bosstemp);
                //触发 boss 自动攻击
                bosstemp.GetComponent<BossBase>().SetBossAttackState(true);
                //AttackManager.GetInstance().BossAutoAttack(bosstemp, transform.gameObject,2.0f);             
            }
            GameObject currboss = colliders[0].gameObject;
            ConstantData.GetInstance().currBoss = currboss;
            //触发主角攻击
            AttackManager.GetInstance().PlayerAttack(transform.gameObject, currboss);
            //触发 boss 自动攻击
            currboss.GetComponent<BossBase>().SetBossAttackState(true);
            //AttackManager.GetInstance().BossAutoAttack(bosstemp, transform.gameObject,2.0f);    
            Debug.Log("Attack the boss");
        }
    }
    /// <summary>
    /// 奔跑动画
    /// </summary>
    public void Run()
    {
        IsAttackWQState(0);
        //animation.Stop();
        animation.clip = clips[0];
        animation.Play();
        animation.CrossFade("ZhanLi_TY", 2.5f);
        ConstantData.GetInstance().BossInfo.SetActive(false);
        ConstantData.GetInstance().BossHUD = null;
    }
    /// <summary>
    /// 攻击动画
    /// </summary>
    public void Attack()
    {
        IsAttackWQState(1);
        animation.Stop();
        animation.clip = clips[1];
        animation.Play();
        animation.CrossFade("ZhanLi_TY", 4.5f);
    }
    //攻击A
    public void AttackA()
    {
        IsAttackWQState(1);
        animation.Stop();
        animation.clip = clips[4];
        animation.Play();
        animation.CrossFade("ZhanLi_TY", 4.5f);
    }
    //攻击B
    public void AttackB()
    {
        IsAttackWQState(1);
        animation.Stop();
        animation.clip = clips[5];
        animation.Play();
        animation.CrossFade("ZhanLi_TY", 4.5f);
    }
    /// <summary>
    /// 死亡动画
    /// </summary>
    public void Dead()
    {
        animation.Stop();
        animation.clip = clips[2];
        animation.Play();
    }
    /// <summary>
    /// 站立动画
    /// </summary>
    public void Stand()
    {
        Debug.Log("Stand");
        animation.CrossFade("ZhanLi_TY", 1.0f);
    }
}
