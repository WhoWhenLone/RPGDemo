using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossItemBase : MonoBehaviour {

	public GameObject BossGameObject;
	// Use this for initialization
	void Start () {

		InitUIBossInfo();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void InitUIBossInfo()
	{
		transform.GetComponentInChildren<UILabel>().text = BossGameObject.GetComponent<BossBase>().GetBossName();
	}
}
