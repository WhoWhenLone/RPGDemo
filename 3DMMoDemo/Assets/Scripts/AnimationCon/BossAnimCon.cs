using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
public class BossAnimCon : MonoBehaviour {

	private Transform player;
	//动画clip数组
	public AnimationClip[] clips;
	//animation 组件
	private Animation animation;
	// Use this for initialization
	void Start () {
		animation = transform.GetComponentInChildren<Animation>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	/// <summary>
	/// 奔跑动画
	/// </summary>
	public void Run()
	{
		//animation.Stop();
		animation.clip = clips[0];
		animation.Play();
		animation.CrossFade("ZhanLi", 2.5f);
	}
	/// <summary>
	/// 攻击动画
	/// </summary>
	public void Attack()
	{
		Debug.Log("Boss Attack Player");
		animation.Stop();
		animation.clip = clips[1];
		animation.Play();
		animation.CrossFade("ZhanLi", 2.0f);
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
		//animation.Stop();
		animation.clip = clips[3];
		animation.Play();
		animation.CrossFade("ZhanLi", 2.0f);
	}
}
