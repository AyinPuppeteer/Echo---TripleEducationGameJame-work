using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//所有Buff的父类
public abstract class Buff
{
    protected string Name;
    public string Name_ => Name;

    protected int Count;
    public int Count_ => Count;

    protected Individual Carrier;//挂载对象
    public void SetCarrier(Individual carrier)
    {
        Carrier = carrier;
    }

    public virtual void Add(Buff buff)
    {
        Count += buff.Count;
    }

    //受伤时
    public virtual void WhenHurt(ref int damage)
    {

    }

    //回合结束时
    public virtual void WhenTurnEnd()
    {

    }

    private void Remove()
    {
        Carrier.Buffs_.Remove(this);
    }
}