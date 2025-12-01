using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burn : Buff
{
    public Burn(int count)
    {
        Name = "»º…’";
        Count = count;
    }

    public override void WhenTurnEnd()
    {
        if (Count == 0) return; 

        Carrier.Hurt(Count);
        Count /= 2;

        if(Count <= 0) Remove();
    }
}