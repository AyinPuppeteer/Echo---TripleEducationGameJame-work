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
    [LabelText("名字")]
    private string Name;
    public string Name_ { get => Name; }

    [SerializeField]
    [TextArea]
    [LabelText("描述")]
    private string Description;
    public string Description_ { get => Description; }

    //强度（攻击牌：伤害 防御牌：盾值 治疗牌：奶量）
    [SerializeField]
    [GUIColor("GetColor")]
    [LabelText("强度")]
    private int Strength;
    public int Strength_ { get => Strength; set => Strength = value; }
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
    [LabelText("类型")]
    private CardType Type = CardType.攻击;
    public CardType Type_ { get => Type; }

    //稀有度
    [SerializeField]
    [LabelText("稀有度")]
    private Rarity Rarity;
    public Rarity Rarity_ { get => Rarity; }

    [SerializeField]
    [LabelText("卡图")]
    private Sprite Image;//图像
    public Sprite Image_ { get => Image; }

    private Card VisualCard;//视觉上的卡片
    public Card VisualCard_ { get => VisualCard; set => VisualCard = value; }

    #region 卡牌效果
    [Header("卡牌效果")]
    [LabelText("战斗开始时效果")]
    [SerializeReference]
    private List<CardEffect_WhenReady> WhenReadyEffect = new();//打出时效果

    [LabelText("打出前效果")]
    [SerializeReference]
    private List<CardEffect_BeforePlay> BeforePlayEffect = new();//打出时效果

    [LabelText("打出时效果")]
    [SerializeReference]
    private List<CardEffect_WhenPlay> WhenPlayEffect = new();//打出时效果

    [FoldoutGroup("词条设置")]
    [LabelText("无声")]
    [SerializeField]
    private bool Silent;
    public bool IsSilent => Silent;

    [FoldoutGroup("词条设置")]
    [LabelText("漫反射")]
    [SerializeField]
    private bool Diffuse;
    public bool IsDiffuse => Diffuse;

    [FoldoutGroup("词条设置")]
    [LabelText("消耗")]
    [SerializeField]
    private bool RunOut;
    public bool CanRunOut => RunOut;
    #endregion

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
        data.Strength = temple.Strength;
        data.Type = temple.Type;
        data.Rarity = temple.Rarity;
        data.Image = temple.Image;

        data.Silent = temple.Silent;
        data.Diffuse = temple.Diffuse;
        data.RunOut = temple.RunOut;

        data.WhenReadyEffect = temple.WhenReadyEffect;
        data.BeforePlayEffect = temple.BeforePlayEffect;
        data.WhenPlayEffect = temple.WhenPlayEffect;
        return data;
    }
    #endregion

    //战斗开始时
    public void WhenReady()
    {
        foreach(var effect in WhenReadyEffect)
        {
            effect.OnWork(VisualCard);
        }
    }

    //打出前
    public void BeforePlay(Individual player, Individual aim)
    {
        foreach (var effect in BeforePlayEffect)
        {
            effect.OnWork(VisualCard, player, aim);
        }
    }

    //当打出时
    public virtual void WhenPlay(Individual player, Individual aim)
    {
        switch (Type)
        {
            case CardType.攻击:
                {
                    aim.Hurt(Strength);
                    break;
                }
            case CardType.防御:
                {
                    aim.AddShield(Strength);
                    break;
                }
            case CardType.治疗:
                {
                    aim.Heal(Strength);
                    break;
                }
        }

        foreach (var effect in WhenPlayEffect)
        {
            effect.OnWork(VisualCard, player, aim);
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