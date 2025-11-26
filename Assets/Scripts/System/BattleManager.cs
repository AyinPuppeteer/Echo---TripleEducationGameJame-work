using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//管理战斗的脚本
public class BattleManager : MonoBehaviour
{
    public Individual Player, Mirror;//玩家和镜像单位

    private CardList Deck;//卡组

    public static BattleManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    //战斗开始
    public void BattleStart()
    {
        Player = new(20);
        Player.WhenDead = Failed;
        Mirror = new(20);
        Mirror.WhenDead = Win;

        Deck = GameManager.Instance.Deck_.Clone();
    }

    //回合开始
    public void TurnStart()
    {

    }

    //开始使用卡牌
    public void Ready()
    {

    }

    //战斗胜利
    public void Win()
    {

    }
    //战斗失败
    public void Failed()
    {

    }
}