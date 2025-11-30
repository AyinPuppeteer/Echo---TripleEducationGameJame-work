using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcidSpray_WhenPlay : CardEffect_WhenPlay
{
    public AcidSpray_WhenPlay()
    {
        Description = "»¤¶Ü¹é0";
    }

    public override void OnWork(Card card, Individual player, Individual aim)
    {
        aim.Shield_ = 0;
    }
}