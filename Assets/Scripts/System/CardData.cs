using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 卡牌数据
/// </summary>
[CreateAssetMenu(menuName = "卡牌数据", fileName = "New CardData")]
public class CardData : ScriptableObject
{
    [SerializeField]
    private string Name;
    public string Name_ { get => Name; }

    [SerializeField]
    [TextArea]
    private string Description;
    public string Description_ { get => Description; }

    //基本数据（攻击牌：伤害 防御牌：盾值 治疗牌：奶量）
    [SerializeField]
    [GUIColor("GetColor")]
    private int Value;
    public int Value_ { get => Value; }
    private Color GetColor()
    {
        switch (Type)
        {
            case CardType.攻击: return Color.red;
            case CardType.防御: return Color.blue;
            case CardType.治疗: return Color.green;
        }
        return Color.white;
    }

    //卡牌类型
    [SerializeField]
    private CardType Type = CardType.攻击;
    public CardType Type_ { get => Type; }

    //稀有度
    [SerializeField]
    private Rarity Rarity;
    public Rarity Rarity_ { get => Rarity; }

    [SerializeField]
    private Sprite Image;//图像
    public Sprite Image_ { get => Image; }

    [SerializeReference]
    private List<CardEffect_WhenPlay> WhenPlayEffect = new();//打出时效果

    private const string PathRoot = "ScriptAssets/卡片数据/";

    #region 生成/克隆卡牌
    /// <summary>
    /// 通过名字获取卡牌属性
    /// </summary>
    public static CardData Cloneby(string name)
    {
        CardData temple = Resources.Load<CardData>(PathRoot + name);
        if (temple == null)
        {
            Debug.LogError($"未能找到该名称的卡片数据: {name}");
            return null;
        }
        else
        {
            return Instantiate(temple);
        }
    }
    /// <summary>
    /// 克隆一张卡片，保持数值和加成
    /// </summary>
    public static CardData Cloneby(CardData temple)
    {
        CardData data = CreateInstance<CardData>();
        data.Name = temple.Name;
        data.Description = temple.Description;
        data.Value = temple.Value;
        data.Type = temple.Type;
        data.Rarity = temple.Rarity;
        data.WhenPlayEffect = temple.WhenPlayEffect;
        return data;
    }
    #endregion

    //当打出时
    public virtual void WhenPlay(Individual player, Individual aim)
    {
        foreach(var effect in WhenPlayEffect)
        {
            effect.OnWork(player, aim);
        }

        switch (Type)
        {
            case CardType.攻击:
                {
                    aim.Hurt(Value);
                    break;
                }
            case CardType.防御:
                {
                    aim.AddShield(Value);
                    break;
                }
            case CardType.治疗:
                {
                    aim.Heal(Value);
                    break;
                }
        }
    }
}

//卡牌的类型
[Serializable]
public enum CardType
{
    攻击, 防御, 治疗
}

//稀有度
public enum Rarity
{
    基础, 稀有
}