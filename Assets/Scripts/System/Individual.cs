using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//管理单位/个体的脚本
public class Individual : MonoBehaviour
{
    private int Health;
    public int Health_ { get => Health; }
    private int MaxHealth;
    public int MaxHealth_ {  get => MaxHealth; }

    private int Shield;//护盾值
    public int Shield_ { get => Shield; }

    [SerializeField]
    private Individual Enemy;//敌对单位
    public Individual Enemy_ { get => Enemy; }

    private readonly List<Buff> Buffs = new();//挂载的Buff列表
    public List<Buff> Buffs_ { get => Buffs; }

    public void SetHealth(int health)
    {
        Health = MaxHealth = health;
    }

    public int Hurt(int damage)
    {
        if(Shield > 0)
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
}