using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundBarrier_WhenPlay : CardEffect_WhenPlay
{
    [LabelText("“Ù’œ≤„ ˝")]
    [Min(1)]
    [SerializeField]
    private int Count = 1;
    public SoundBarrier_WhenPlay()
    {
        Description = "“Ù’œX";
    }

    public override void OnWork(Card card, Individual player, Individual aim)
    {
        aim.AddBuff(new SoundBarrier(Count));
    }
}