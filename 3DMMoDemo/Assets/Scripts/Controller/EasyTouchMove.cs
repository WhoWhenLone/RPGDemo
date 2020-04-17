using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EasyTouchMove : MonoBehaviour {

	public GameObject FollowedCamera;
	CharacterController m_Controller;
	private GameObject playermodel;
	public float DISTANCE = 20f;
	void OnEnable()
	{
		EasyJoystick.On_JoystickMove += On_JoysticMove;
		EasyJoystick.On_JoystickMoveEnd += On_JoysticMoveEnd;
	}
	// Use this for initialization
	void Start () {
		playermodel = transform.Find("ZhanShiNan80").gameObject;
		m_Controller = GetComponent<CharacterController>();
		FollowedCamera = Camera.main.transform.gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		CheckBossActive();
	}


    
    void On_JoysticMove(MovingJoystick move)
	{
		float x = move.joystickAxis.x;
		float y = move.joystickAxis.y;

		float angle = 0;
		Vector3 mDir = Vector3.zero;

		Vector3 dir = Vector3.zero;
		//计算遥感偏移角度atan反正弦函数 得到弧度 在转换为 角度
		if(y > 0)
		{
			angle = Mathf.Atan(x / y) * Mathf.Rad2Deg;
		}
		if(x < 0 && y < 0)
		{
			angle = Mathf.Atan(x / y) * Mathf.Rad2Deg - 180f;
		}
		if(x > 0&& y < 0)
		{
			angle = Mathf.Atan(x / y) * Mathf.Rad2Deg + 180f;
		}
		if(angle != 0)
		{
			ConstantData.GetInstance().Player.GetComponent<NavMeshAgent>().enabled = false;
			ConstantData.GetInstance().Player.GetComponent<PlayerBase>().SetPlayerAttackState(false);
			UIController.GetInstance().StopPlayerAutoAttack();
		}
		//计算当前人物正方向
		mDir =  new Vector3(transform.position.x - FollowedCamera.transform.position.x, 0, transform.position.z-FollowedCamera.transform.position.z);
		//计算当前人物移动方向 即正方向转angle度
		dir = Quaternion.AngleAxis(angle, transform.up) * mDir;
		//根据当前移动方向 计算人物角度值
		Quaternion newRotation = Quaternion.LookRotation(dir);
		//人物转向
		transform.rotation = Quaternion.RotateTowards(transform.rotation, newRotation, 100);
		transform.GetComponent<PlayerAnimCon>().Run();
		m_Controller.SimpleMove(dir * 1.0f);
	}

	void On_JoysticMoveEnd(MovingJoystick move)
	{
		transform.GetComponent<PlayerAnimCon>().Stand();
	}

    public void CheckBossActive()
    {
        //Debug.Log("检测boss 显示状态");
        int len = ConstantData.GetInstance().BossList.Count;
        for (int i = 0; i < len; i++)
        {
            CheckDistance(ConstantData.GetInstance().BossList[i]);
        }
    }

    private void CheckDistance(GameObject boss)
    {
        float distance = Vector3.Distance(transform.position, boss.transform.position);
        if (Mathf.Abs(distance) < DISTANCE)
        {
            boss.SetActive(true);
        }
        else
        {
            boss.SetActive(false);
        }
    }
}
