using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//管理战斗的脚本
public class BattleManager : MonoBehaviour
{
    public CanvasGroup Canva;
    public void SetActive(bool b)
    {
        Canva.interactable = b;
        Canva.blocksRaycasts = b;
    }

    private Individual Player, Mirror;//玩家和镜像单位
    public Individual Player_ { get => Player; }
    public Individual Mirror_ { get => Player_; }

    private CardList Deck;//卡组
    private CardList Tomb;//墓地

    [SerializeField]
    private CommandArea PlayerHand, MirrorHand;

    [SerializeField]
    private GameObject CardOb;//卡片物体

    private GameObject ob;

    public static BattleManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        SetActive(false);
    }

    //战斗开始
    public void BattleStart()
    {
        SetActive(true);

        GameManager.Instance.Level_++;//轮次增加

        Player = new(20)
        {
            WhenDead = Failed
        };
        Mirror = new(20)
        {
            WhenDead = Win
        };
        Deck = GameManager.Instance.Deck_.Clone();
        Tomb = new();

        TurnStart();
    }

    //回合开始
    public void TurnStart()
    {
        for(int i = 1; i <= 10; i++)
        {
            DOTween.To(() => 0, x => { }, 0, 0.1f * i).OnComplete(() =>
            {
                ob = Instantiate(CardOb, PlayerHand.transform);//生成卡片
                Card card = ob.GetComponent<Card>();
                card.Initialize(Deck.Draw());
                PlayerHand.AddCard(card);//加入手牌区域
            });
        }
    }

    //开始使用卡牌
    public void Ready()
    {

    }

    //战斗胜利
    public void Win()
    {
        SetActive(false);
        ShopManager.Instance.Open();//展示商店
        ShopManager.Instance.Init();
    }
    //战斗失败
    public void Failed()
    {

    }
}