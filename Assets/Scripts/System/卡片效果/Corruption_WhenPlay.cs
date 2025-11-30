using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Corruption_WhenPlay : CardEffect_WhenPlay
{
    public Corruption_WhenPlay()
    {
        Description = "ÉúÃüÖµ¼õ°ë";
    }

    public override void OnWork(Card card, Individual player, Individual aim)
    {
        aim.Health_ = aim.Health_ / 2;
    }
}