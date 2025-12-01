using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burn_BeforePlay : CardEffect_BeforePlay
{
    [LabelText("»º…’≤„ ˝")]
    [Min(1)]
    [SerializeField]
    private int Count;
    public Burn_BeforePlay()
    {
        Description = "»º…’X";
    }

    public override void OnWork(Card card, Individual player, Individual aim)
    {
        aim.AddBuff(new Burn(Count));
    }
}