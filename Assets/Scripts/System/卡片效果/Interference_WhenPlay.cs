using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intereference : CardEffect_WhenPlay
{
    [LabelText("干扰数目")]
    [Min(1)]
    [SerializeField]
    private int Count = 1;

    public Intereference()
    {
        Description = "干涉X";
    }

    public override void OnWork(Card card, Individual player, Individual aim)
    {
        for (int i = card.Index_ + 1; i <= card.Index_ + Count; i++)
        {
            if (i == 10 || i >= 20) break;
            Card c = BattleManager.Instance.GetHandAt(i);
            if(c != null) c.Ban();
        }
    }
}
