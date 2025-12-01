using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dodge : Buff
{
    public Dodge(int count)
    {
        Name = "иа╠э";
        Count = count;
    }

    public override void WhenHurt(ref int damage)
    {
        damage = 0;
        BattleManager.Instance.SummonText("иа╠э", Color.grey, Carrier.transform.position);
        Count--;
        if (Count <= 0) Remove();
    }
}