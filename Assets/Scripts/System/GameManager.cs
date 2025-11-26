using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//管理游戏进程的脚本
public class GameManager : MonoBehaviour
{
    #region 货币
    private int Coin;
    public int Coin_ { get => Coin; }

    public void AddCoin(int delta)
    {
        Coin += delta;
    }
    public void DelCoin(int delta)
    {
        if(delta > Coin)
        {
            Debug.LogError("没有这么多货币！");
            return;
        }
        Coin -= delta;
    }
    #endregion

    private CardList Deck = new();//牌库
    public CardList Deck_ { get => Deck; }

    private int Turn;//第几次战斗
    public int Turn_ { get => Turn; }

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    //初始化
    public void Init(CardList basedeck)//basecard表示初始卡组
    {
        Deck = basedeck;
    }
}