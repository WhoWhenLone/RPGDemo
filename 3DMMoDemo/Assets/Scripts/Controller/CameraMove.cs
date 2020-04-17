using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    //旋转速度
    public float sensitivityMouse = 40f;
    //旋转大小
    public float rotateSpeed = 0.1f;
    public Transform target;
    //观察距离  
    public float Distance = 3F;
    public float InitDis = 3.0f;
    //观察高度
    public float hight = 1.3f;
    //旋转角度  
    private float mX = 0.0F;
    private float mY = 35F;
    //角度限制  
    private float MinLimitY = -50;
    private float MaxLimitY = 85;
    //是否启用差值  
    public bool isNeedDamping = true;
    //速度  
    public float Damping = 10F;

    // Use this for initialization
    void Start()
    {
        target = ConstantData.GetInstance().Player.transform.Find("player_camera_target").transform;
    }

    // Update is called once per frame
    void Update()
    {
    }

    void LateUpdate()
    {
        if(Input.GetMouseButton(1))
        {
            //获取鼠标输入  
            mX += Input.GetAxis("Mouse X") * sensitivityMouse * rotateSpeed;
            mY -= Input.GetAxis("Mouse Y") * sensitivityMouse * rotateSpeed;
        }
        //范围限制  
        mY = ClampAngle(mY, MinLimitY, MaxLimitY);
        //计算摄像机距离
        ChangeDistance();

        //重新计算位置和角度  
        Quaternion mRotation = Quaternion.Euler(mY, mX, 0);
        //重新计算摄像机位置
        Vector3 mPosition = mRotation * new Vector3(0.0F, 0f, -Distance) + target.position;
        //设置相机的角度和位置  
        if (isNeedDamping)
        {
            //球形插值  
            transform.rotation = Quaternion.Lerp(transform.rotation, mRotation, Time.deltaTime * Damping);
            //线性插值  
            transform.position = Vector3.Lerp(transform.position, mPosition, Time.deltaTime * Damping);
        }
        else
        {
            transform.rotation = mRotation;
            transform.position = mPosition;
        }
    }
    /// <summary>
    /// 设置范围
    /// </summary>
    /// <param name="angle"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    private float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360) angle += 360;
        if (angle > 360) angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }

    /// <summary>
    /// 根据摄像相机角度 动态修改摄像机距离
    /// </summary>
    private void ChangeDistance()
    {
        if (mY < 0)
        {
            Distance = InitDis - Distance * (mY / MinLimitY);
            if (mY <= -40)
            {
                Distance = 1.5f;
            }
        }
        else
        {
            Distance = InitDis;
        }
    }
}
