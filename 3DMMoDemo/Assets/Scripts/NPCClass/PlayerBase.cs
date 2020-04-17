using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : MonoBehaviour 
{
    public GameObject PlayerInfo;
    public GameObject BossInfo;

    public GameObject PlayerHud;
    public GameObject BossHud;
    public string name;
    public float blood;
    public float attack;

    private string player_name;
    private float player_blood;
    private float player_attack;
    private bool player_attack_state = false;
    private bool player_state = true;
    private bool player_auto_attack = false;

    void Start()
    {
        InitPlayer();
    }
    /// <summary>
    /// 初始化player数据
    /// </summary>
    public void InitPlayer()
    {
        SetPlayerName(name);
        SetPlayerBlood(blood);
        SetPlayerAttack(attack);
        SetPlayerState(true);
        InitPlayerUI();
        InitBossUI();
    }
    private void InitPlayerUI()
    {
        //初始化 血量
        Debug.Log("value  " + ConstantData.GetInstance().PlayerInfo.GetComponentInChildren<UISlider>().value);
        ConstantData.GetInstance().PlayerInfo.GetComponentInChildren<UISlider>().value = GetPlayerBlood() /100f;
    }
    private void InitBossUI()
    {
        ConstantData.GetInstance().BossInfo.SetActive(false);
    }
    public void SetPlayerName(string name)
    {
        this.player_name = name;
    }
    public string GetPlayerName()
    {
        return player_name;
    }

    public void SetPlayerBlood(float blood)
    {
        this.player_blood = blood;
    }
    public float GetPlayerBlood()
    {
        return player_blood;
    }

    public void SetPlayerAttack(float attack)
    {
        this.player_attack = attack;
    }
    public float GetPlayerAttack()
    {
        return player_attack;
    }

    public void SetPlayerState(bool state)
    {
        this.player_state = state;
    }
    public bool GetPlayerState()
    {
        return player_state;
    }

    public void SetPlayerAttackState(bool state)
    {
        this.player_attack_state = state;
    }
    public bool GetPlayerAttackState()
    {
        return player_attack_state;
    }

    public void SetPlayerAutoAttackState(bool state)
    {
        player_auto_attack = state;
    }
    public bool GetPlayerAutoAttackState()
    {
        return player_auto_attack;
    }
}
