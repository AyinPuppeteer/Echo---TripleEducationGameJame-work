using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//所有Buff的父类
public abstract class Buff
{
    private string Name;

    private int Count;

    public Individual Carrier;//挂载对象

    //受伤时
    public virtual void WhenHurt(ref int damage)
    {

    }

    //回合结束时
    public virtual void WhenTurnEnd()
    {

    }
}