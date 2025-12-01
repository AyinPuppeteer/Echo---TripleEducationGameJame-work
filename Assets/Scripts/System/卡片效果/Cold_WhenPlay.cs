using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cold_WhenPlay : CardEffect_WhenPlay
{
    [LabelText("∫Æ¿‰≤„ ˝")]
    [Min(1)]
    [SerializeField]
    private int Count;

    public Cold_WhenPlay()
    {
        Description = "∫Æ¿‰X";
    }

    public override void OnWork(Card card, Individual player, Individual aim)
    {
        aim.AddBuff(new Cold(Count));
    }
}