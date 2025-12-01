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

    private bool DelTag = false;//删除标记（为true时会被挂载单位消除）
    public bool DelTag_ => DelTag;

    protected Individual Carrier;//挂载对象
    public void SetCarrier(Individual carrier)
    {
        Carrier = carrier;
    }

    public virtual void Add(Buff buff)
    {
        Count += buff.Count;
    }

    //战斗开始时
    public virtual void WhenReady()
    {

    }

    //受伤时
    public virtual void WhenHurt(ref int damage)
    {

    }

    //回合结束时
    public virtual void WhenTurnEnd()
    {

    }

    public void Remove()
    {
        DelTag = true;
    }
}