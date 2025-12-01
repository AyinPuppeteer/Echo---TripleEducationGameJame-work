using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundBarrier : Buff
{
    public SoundBarrier(int count)
    {
        Name = "ÒôÕÏ";
        Count = count;
    }

    public override void WhenHurt(ref int damage)
    {
        if(Count > damage)
        {
            Count -= damage;
            Carrier.Enemy_.Hurt(damage);
            damage = 0;
        }
        else
        {
            damage -= Count;
            Carrier.Enemy_.Hurt(Count);
            Count = 0;
            Remove();
        }
    }
}