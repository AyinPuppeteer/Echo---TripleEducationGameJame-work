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
    public void AddShield(int shield) => Shield += shield;

    public Individual Enemy;//敌对单位

    public Action WhenDead;//死亡时效果

    public Individual(int health)
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
        if(Health <= 0)
        {
            if(WhenDead != null) WhenDead();
        }
        return damage;
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
        return heal;
    }
}