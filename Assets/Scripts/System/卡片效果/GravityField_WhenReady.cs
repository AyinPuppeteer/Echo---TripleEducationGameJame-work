using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityField_WhenReady : CardEffect_WhenReady
{
    public GravityField_WhenReady()
    {
        Description = "½ûÓÃ¶ÔÃæ¿¨Æ¬";
    }

    public override void OnWork(Card card)
    {
        int inverse_index = card.Index_ < 10 ? card.Index_ + 10 : card.Index_ - 10;
        Card c = BattleManager.Instance.GetHandAt(inverse_index);
        if(c != null) c.Ban();
    }
}