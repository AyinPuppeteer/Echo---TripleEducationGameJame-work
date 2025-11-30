using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Echo_BeforePlay : CardEffect_BeforePlay
{
    [LabelText("回响数目")]
    [Min(1)]
    [SerializeField]
    private int Count = 1;

    public Echo_BeforePlay()
    {
        Description = "回响X";
    }

    public override void OnWork(Card card, Individual player, Individual aim)
    {
        for(int i = 1; i <= Count; i++)
        {
            BattleManager.Instance.ActionList_.Add(() => {
                BattleManager.Instance.PlayCard(card, player);
                return true;
            });
        }
    }
}
