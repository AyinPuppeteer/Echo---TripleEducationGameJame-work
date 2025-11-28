using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//管理单位/个体的脚本
public class Individual
{
    private int Health;
    public int Health_ { get => Health; }
    private int MaxHealth;
    public int MaxHealth_ {  get => MaxHealth; }

    private int Shield;//护盾值
    public int Shield_ { get => Shield; }
    public void AddShield(int shield) => Shield += shield;

    public Individual Enemy;//敌对单位

    public Individual(int health) : base()
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