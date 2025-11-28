using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//卡牌异能
public enum Ability
{
    无声, 干涉, 回响, 音障, 燃烧, 漫反射, 消耗, 闪避, 共振, 寒冷
}

public class AbilityPack
{
    public string Name;
    public string Description;
    public Color Color;

    public AbilityPack(string name, string description, Color color)
    {
        Name = name; 
        Description = description; 
        Color = color;
    }
}

//用于获取异能解释的字典
public class AbilityDictionary
{
    private static Dictionary<Ability, AbilityPack> Descriptions;

    private static void Initialize()
    {
        Descriptions = new();
        Add(Ability.无声, "这个魔法不会被敌人复制。", Color.white);
        Add(Ability.干涉, "后X个魔法不会生效。", Color.white, "干涉X");
        Add(Ability.回响, "回响X：这个魔法会重复发动X次。", Color.white, "回响X");
        Add(Ability.音障, "音障X：受到攻击时抵消X点伤害，然后反弹给攻击者。", Color.white, "音障X");
        Add(Ability.燃烧, "回合结束时，造成X点伤害，然后层数减半。", Color.white, "燃烧X");
        Add(Ability.漫反射, "这张牌被敌人复制时，其强度变为0。", Color.white);
        Add(Ability.消耗, "使用后，这张卡牌在本轮移除而不进入墓地。", Color.white);
        Add(Ability.闪避, "免除接下来受到的X次伤害。", Color.white, "闪避X");
        Add(Ability.共振, "前一张卡片与这张卡片类型相同时触发。", Color.white);
        Add(Ability.寒冷, "最前方的X张卡牌不会生效。", Color.white, "寒冷X");
    }

    private static void Add(Ability ability, string description, Color color, string name = "")
    {
        Descriptions.Add(ability, new(name == "" ? ability.ToString() : name, description, color));
    }

    public static AbilityPack Find(string name)
    {
        if(Descriptions == null) Initialize();
        if(Enum.TryParse(name, out Ability ability))
        {
            if (Descriptions.ContainsKey(ability)) return Descriptions[ability];
            else Debug.LogError($"字典中没有该能力的信息：{name}!");
        }
        else Debug.LogError($"未查询到该能力：{name}!");
        return null;
    }
}