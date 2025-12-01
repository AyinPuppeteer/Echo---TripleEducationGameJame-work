using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//管理单位/个体的脚本
public class Individual : MonoBehaviour
{
    private int Health;
    public int Health_ { get => Health; set => Health = value; }
    private int MaxHealth;
    public int MaxHealth_ {  get => MaxHealth; }

    private int Shield;//护盾值
    public int Shield_ { get => Shield; set => Shield = value; }

    [SerializeField]
    private Individual Enemy;//敌对单位
    public Individual Enemy_ { get => Enemy; }

    #region Buff管理
    private readonly List<Buff> Buffs = new();//挂载的Buff列表
    public List<Buff> Buffs_ { get => Buffs; }

    [SerializeField]
    private Transform BuffField;//Buff图标区域
    public Transform BuffField_ => BuffField;

    //添加Buff
    public void AddBuff(Buff buff)
    {
        foreach(var b in Buffs)
        {
            if (b.Name_ == buff.Name_)
            {
                b.Add(buff);
                return;
            }
        }
        Buffs.Add(buff);
        buff.SetCarrier(this);
        BuffIcon_Creator.Instance.CreateBuffIcon(buff, BuffField);
    }

    public void BuffRefresh()
    {
        List<Buff> DelList = new();//删除队列
        foreach(var b in Buffs)
        {
            if(b.DelTag_) DelList.Add(b);
        }
        foreach(var b in DelList)
        {
            Buffs.Remove(b);
        }
    }
    #endregion

    public void Init(int health)
    {
        Health = MaxHealth = health;
        Shield = 0;

        foreach(var buff in Buffs) buff.Remove(); //移除所有Buff
    }

    public int Hurt(int damage)
    {
        foreach(var buff in Buffs)
        {
            buff.WhenHurt(ref damage);
        }

        if (Shield > 0)
        {
            if(Shield >= damage)
            {
                Shield -= damage;
                return 0;
            }
            else
            {
                Shield = 0;
                damage -= Shield;
            }
        }
        Health -= damage;
        BattleManager.Instance.SummonNumber(damage, Color.red, transform.position);
        return damage;
    }

    public void AddShield(int shield)
    {
        Shield += shield;
        BattleManager.Instance.SummonNumber(shield, new(0, 0.4f, 0.7f), transform.position);
    }

    public int Heal(int heal)
    {
        if (Health + heal >= MaxHealth)
        {
            heal = MaxHealth - Health;
            Health = MaxHealth;
        }
        else
        {
            Health += heal;
        }
        BattleManager.Instance.SummonNumber(heal, Color.green, transform.position);
        return heal;
    }

    public void Ready()
    {
        foreach(var buff in Buffs)
        {
            buff.WhenReady();
        }
    }

    public void TurnEnd()
    {
        foreach(var buff in Buffs)
        {
            buff.WhenTurnEnd();
        }

        Shield = 0;//回合结束护盾消失
    }

    private void LateUpdate()
    {
        BuffRefresh();
    }
}