using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resonate_BeforePlay : CardEffect_BeforePlay
{
    [LabelText("共鸣时效果")]
    [SerializeReference]
    private List<CardEffect_BeforePlay> Effects = new();

    public Resonate_BeforePlay()
    {
        Description = "共鸣X";
    }

    public override void OnWork(Card card, Individual player, Individual aim)
    {
        Card c = card.Last;
        if(c != null && c.CardData.Type_ == card.CardData.Type_)
        {
            foreach(var effect in Effects) effect.OnWork(card, player, aim);
        }
    }
}