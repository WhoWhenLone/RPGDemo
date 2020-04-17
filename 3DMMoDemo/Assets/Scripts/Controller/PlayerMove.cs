using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class PlayerMove : MonoBehaviour {
	public Transform FollowedCamera;
	public float Speed;
	public float gravity = 20f;
	public float margin = 0.1f;

	//野怪显示范围
	public float DISTANCE = 5f;
	CharacterController m_Controller;
	// Use this for initialization
	void Start () {
		m_Controller = GetComponent<CharacterController>();
		//CheckBossActive();
		FollowedCamera = Camera.main.transform;
	}
	
	// Update is called once per frame
	void Update () {
		//主角死亡状态直接返回
		if(!transform.gameObject.GetComponent<PlayerBase>().GetPlayerState())
		{
			return;
		}
		//移动方向
		Vector3 mDir = Vector3.zero;
		float h = Input.GetAxis("Horizontal");
		float v = Input.GetAxis("Vertical");

		if (h != 0 || v!= 0)
		{
			if (Input.GetKey(KeyCode.W))
			{
				mDir = new Vector3(FollowedCamera.transform.forward.x, 0, FollowedCamera.transform.forward.z);
				Quaternion newRotation = Quaternion.LookRotation(mDir);
				transform.rotation = Quaternion.RotateTowards(transform.rotation, newRotation, 100);
			}
			else if (Input.GetKey(KeyCode.S))
			{
				mDir = new Vector3(-FollowedCamera.transform.forward.x, 0, -FollowedCamera.transform.forward.z);
				Quaternion newRotation = Quaternion.LookRotation(mDir);
				transform.rotation = Quaternion.RotateTowards(transform.rotation, newRotation, 100);
			}
			if (Input.GetKey(KeyCode.D))
			{
				mDir = new Vector3(FollowedCamera.transform.right.x, 0, FollowedCamera.transform.right.z);
				Quaternion newRotation = Quaternion.LookRotation(mDir);
				transform.rotation = Quaternion.RotateTowards(transform.rotation, newRotation, 100);
			}
			else if (Input.GetKey(KeyCode.A))
			{
				mDir = new Vector3(-FollowedCamera.transform.right.x, 0, -FollowedCamera.transform.right.z);
				Quaternion newRotation = Quaternion.LookRotation(mDir);
				transform.rotation = Quaternion.RotateTowards(transform.rotation, newRotation, 100);
			}
			
			transform.GetComponent<PlayerAnimCon>().Run();
            ConstantData.GetInstance().Player.GetComponent<NavMeshAgent>().enabled = false;
            ConstantData.GetInstance().Player.GetComponent<PlayerBase>().SetPlayerAttackState(false);
            UIController.GetInstance().StopPlayerAutoAttack();
			m_Controller.SimpleMove(transform.forward*1.5f * Speed);
		}
		//检测boss 显示状态
		CheckBossActive();
	}

	private void CheckBossActive()
	{
		//Debug.Log("检测boss 显示状态");
		int len = ConstantData.GetInstance().BossList.Count;
		for (int i = 0; i < len; i++)
		{
			CheckDistance(ConstantData.GetInstance().BossList[i],DISTANCE);
		}
	}

	private void CheckDistance(GameObject boss,float dis)
	{
		float distance = Vector3.Distance(transform.position, boss.transform.position);
		//Debug.Log("distance :" + distance+"  DISTANCE: "+DISTANCE);
		if(Mathf.Abs(distance) < dis)
		{
			boss.SetActive(true);
		}
		else
		{
			boss.SetActive(false);
		}
	}

   
}
