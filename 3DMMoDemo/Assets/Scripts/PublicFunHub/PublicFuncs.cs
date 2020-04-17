using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class PublicFuncs : MonoBehaviour {

	public static PublicFuncs instance;
	public static PublicFuncs GetInstance()
	{
		return instance;
	}
	// Use this for initialization
	void Start () {
		if(instance == null)
		{
			instance = this;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	/// <summary>
	/// 延迟函数
	/// </summary>
	/// <param name="time"></param>
	/// <param name="action"></param>
	public void WaitTimeDoFun(float time,UnityAction action)
	{
		StartCoroutine(WaitTime(time, action));
	}
	private IEnumerator WaitTime(float time, UnityAction action)
	{
		yield return new WaitForSeconds(time);
		action();
	}
}
