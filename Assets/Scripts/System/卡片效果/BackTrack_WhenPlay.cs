using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackTrack_WhenPlay : CardEffect_WhenPlay
{
    private int RecordHealth;//记录的血量

    public BackTrack_WhenPlay()
    {
        Description = "记录血量并在回合结束时恢复";
    }

    public override void OnWork(Card card, Individual player, Individual aim)
    {
        RecordHealth = aim.Health_;
        BattleManager.Instance.TurnEndAction_.Add(() =>
        {
            aim.Health_ = RecordHealth;
        });
    }
}