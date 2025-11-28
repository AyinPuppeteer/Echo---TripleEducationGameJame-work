using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//卡牌异能
public enum Ability
{
    无声, 干涉, 回响, 音障, 燃烧, 漫反射
}

//用于获取异能解释的字典
public class AbilityDictionary
{
    private static Dictionary<Ability, string> Descriptions;

    private static void Initialize()
    {
        Descriptions.Add(Ability.无声, "无声：这个魔法不会被敌人复制。");
        Descriptions.Add(Ability.干涉, "干涉：相邻的两个魔法无法生效。");
        Descriptions.Add(Ability.回响, "回响X：这个魔法会重复发动X次。");
        Descriptions.Add(Ability.音障, "音障X：受到攻击时抵消X点伤害，然后反弹给敌人。");
        Descriptions.Add(Ability.燃烧, "燃烧X：回合结束时，造成X点伤害，然后层数减半。");
        Descriptions.Add(Ability.漫反射, "漫反射：这张牌被敌人复制时，其强度变为0。");
    }

    public static void Describe(Ability ability)
    {
        if (Descriptions == null) Initialize();
    }
}