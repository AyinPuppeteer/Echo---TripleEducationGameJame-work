using DG.Tweening;
using System;
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

    [SerializeField]
    private Individual Player, Mirror;//玩家和镜像单位
    public Individual Player_ { get => Player; }
    public Individual Mirror_ { get => Player_; }

    private CardList Deck;//卡组
    private CardList Tomb;//墓地

    #region 手牌管理
    [SerializeField]
    private CommandArea PlayerHand, MirrorHand;

    public Card GetHandAt(int id)
    {
        if (id < 0) return null;
        if (id < 10) return PlayerHand[id];
        else return MirrorHand[id - 10];
    }

    private void AddtoPlayerHand(CardData carddata)
    {
        ob = Instantiate(CardOb, PlayerHand.transform);//生成卡片
        Card card = ob.GetComponent<Card>();
        card.Initialize(carddata);
        PlayerHand.AddCard(card);//加入手牌区域
        card.PlayAppearAnimation();
    }
    private void AddtoMirrorHand(CardData carddata)
    {
        ob = Instantiate(CardOb, PlayerHand.transform);//生成卡片
        Card card = ob.GetComponent<Card>();
        card.Initialize(carddata);
        MirrorHand.AddCard(card);//加入手牌区域
        card.PlayAppearAnimation();
        card.SetInteractable(false);
        DOTween.To(() => 0, x => { }, 0, 0.2f).OnComplete(() => card.PlayRippleEffect());
    }
    #endregion

    [SerializeField]
    private GameObject CardOb;//卡片物体

    private GameObject ob;

    private bool CardPlaying;//是否正在打出卡牌
    private float CardPlayTimer, CardPlayTime = 0.4f;//出牌间隔计时器和出牌间隔

    private int CardIndex;//当前打出的卡牌下标

    private readonly List<Func<bool>> ActionList = new();//行动列表
    public List<Func<bool>> ActionList_ => ActionList;

    #region 回响机制
    private readonly List<CardData> EchoList = new();//回响序列（镜像的复制序列）
    /// <summary>
    /// 让镜像复制指定的魔法
    /// </summary>
    public void Echo(CardData card)
    {
        if(EchoList.Count < 10)
        {
            CardData clonecard = CardData.Cloneby(card);
            if(card.IsDiffuse) clonecard.Strength_ = 0;
            EchoList.Add(clonecard);
        }
    }
    #endregion

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

        GameManager.Instance.Level_++;//关卡数增加

        Player.SetHealth(30);
        Mirror.SetHealth(30);

        Deck = GameManager.Instance.Deck_.Clone();
        Tomb = new();

        TurnStart();
    }

    //回合开始
    public void TurnStart()
    {
        PlayerHand.ClearCommandArea();
        MirrorHand.ClearCommandArea();

        for(int i = 1; i <= 10; i++)
        {
            if(Deck.Count <= 0)
            {
                if (Tomb.Count >= 1)//卡组空将墓地洗回
                {
                    Deck.AddRange(Tomb);
                    Tomb.Clear();
                }
                else break;//如果墓地也空则结束抽牌
            }
            CardData carddata = Deck.Draw();
            DOTween.To(() => 0, x => { }, 0, 0.2f + 0.1f * i).OnComplete(() => AddtoPlayerHand(carddata));
        }
        for (int i = 0; i < EchoList.Count; i++)
        {
            CardData carddata = EchoList[i];
            DOTween.To(() => 0, x => { }, 0, 0.3f + 0.1f * i).OnComplete(() => AddtoMirrorHand(carddata));
        }
    }

    private void Update()
    {
        if (CardPlaying)
        {
            if((CardPlayTimer += Time.deltaTime) >= CardPlayTime)
            {
                CardPlayTimer = 0;

                while (ActionList.Count > 0)
                {
                    Func<bool> action = ActionList[0];
                    ActionList.RemoveAt(0);
                    if (action()) return;//行动后直接跳出
                }

                if(++CardIndex > 10)//回合结束
                {
                    CardPlaying = false;
                    CardIndex = 0;
                    foreach (var card in PlayerHand)
                    {
                        card.PlayDisappearAnimation();
                        if(!card.CardData.CanRunOut) Tomb.Add(card.CardData);
                    }
                    foreach (var card in MirrorHand)
                    {
                        card.PlayDisappearAnimation();
                    }

                    if (Player.Health_ < 0) Failed();
                    else if (Mirror.Health_ < 0) Win();
                    else
                    {
                        DOTween.To(() => 0, x => { }, 0, 0.4f).OnComplete(TurnStart);
                    }
                }
                else
                {
                    Card playercard = null, mirrorcard = null;
                    if(PlayerHand.Count >= CardIndex)
                    {
                        playercard = PlayerHand[CardIndex - 1];
                        if (Player.Health_ > 0 && playercard.CanUse) playercard.CardData.BeforePlay(Player, Mirror);
                    }
                    if (MirrorHand.Count >= CardIndex)
                    {
                        mirrorcard = MirrorHand[CardIndex - 1];
                        if (Mirror.Health_ > 0 && mirrorcard.CanUse) mirrorcard.CardData.BeforePlay(Mirror, Player);
                    }

                    if (playercard != null)
                    {
                        PlayCard(playercard, Player);
                    }
                    if (mirrorcard != null)
                    {
                        PlayCard(mirrorcard, Mirror);
                    }
                }
            }
        }
    }

    public void PlayCard(Card card, Individual player)
    {
        if (player.Health_ > 0)
        {
            if (card.CanUse)
            {
                card.Play(player);//血量大于0则使用卡
                if (player == Player && !card.CardData.IsSilent) Echo(card.CardData);//回响序列添加
            }
        }
    }

    //开始使用卡牌
    public void Ready()
    {
        CardPlaying = true;
        EchoList.Clear();

        foreach(var card in PlayerHand)
        {
            card.SetInteractable(false);
            card.CardData.WhenReady();
        }
        foreach(var card in MirrorHand)
        {
            card.CardData.WhenReady();
        }
    }

    //战斗胜利
    public void Win()
    {
        SetActive(false);
        ShopManager.Instance.Open();//展示商店
        ShopManager.Instance.Init();

        GameManager.Instance.SaveGame();
    }
    //战斗失败
    public void Failed()
    {

    }

    #region 生成数字特效
    [SerializeField]
    private GameObject NumberOb;//数字物体

    public void SummonNumber(int num, Color c, Vector3 pos)
    {
        ob = Instantiate(NumberOb, pos, Quaternion.identity, transform);
        ob.GetComponent<NumberText>().Init(num, c);
    }
    #endregion
}