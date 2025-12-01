using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cold : Buff
{
    public Cold(int count)
    {
        Name = "∫Æ¿‰";
        Count = count;
    }

    public override void WhenReady()
    {
        if(Carrier == BattleManager.Instance.Player_)
        {
            for(int i = 0; i < Count; i++)
            {
                Card card = BattleManager.Instance.GetHandAt(i);
                if(card != null) card.Ban();
            }
        }
        else if(Carrier == BattleManager.Instance.Mirror_)
        {
            for (int i = 10; i < Count + 10; i++)
            {
                Card card = BattleManager.Instance.GetHandAt(i);
                if (card != null) card.Ban();
            }
        }

        Remove();
    }
}