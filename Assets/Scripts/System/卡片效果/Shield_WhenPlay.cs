using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield_WhenPlay : CardEffect_WhenPlay
{
    [LabelText("护盾值")]
    [SerializeField]
    private int Strength;

    public Shield_WhenPlay()
    {
        Description = "获得X点护盾";
    }

    public override void OnWork(Card card, Individual player, Individual aim)
    {
        aim.AddShield(Strength);
    }
}