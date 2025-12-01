using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dodge_WhenPlay : CardEffect_WhenPlay
{
    [LabelText("…¡±‹≤„ ˝")]
    [Min(1)]
    [SerializeField]
    private int Count = 1;
    public Dodge_WhenPlay()
    {
        Description = "…¡±‹X";
    }

    public override void OnWork(Card card, Individual player, Individual aim)
    {
        aim.AddBuff(new Dodge(Count));
    }
}