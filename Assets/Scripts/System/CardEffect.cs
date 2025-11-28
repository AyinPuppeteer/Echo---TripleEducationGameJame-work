using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 卡牌效果
/// </summary>
[Serializable]
public abstract class CardEffect : IComparable<CardEffect>
{
    //描述（用于判断是否重复）
    public string Description = "";

    public int CompareTo(CardEffect other)
    {
        return Description.CompareTo(other.Description);
    }
}

[Serializable]
public abstract class CardEffect_WhenPlay : CardEffect
{
    public abstract void OnWork(Individual player, Individual aim);
}