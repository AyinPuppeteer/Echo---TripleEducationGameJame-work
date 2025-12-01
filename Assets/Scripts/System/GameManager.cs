using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    private int Level;//第几次战斗
    public int Level_ { get => Level; set => Level = value; }

    private bool IsBattle;//是否处于战斗中

    private float Luckiness;//幸运（影响商店刷新物品的质量）
    public float Luckiness_ { get => Luckiness; }

    [SerializeField]
    private TextMeshProUGUI LevelText;
    [SerializeField]
    private TextMeshProUGUI CoinText;

    [SerializeField]
    private CanvasGroup ResultCanva;
    [SerializeField]
    private Image WinImage, FailImage;//胜利图片和失败图片

    public static GamePack Pack;

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if(Pack == null)
        {
            CardList basedeck = new()
            {
                "基础攻击",
                "基础攻击",
                "基础攻击",
                "基础攻击",
                "基础攻击",
                "基础攻击",
                "基础攻击",
                "基础防御",
                "基础防御",
                "基础防御",
                "基础防御",
                "基础治疗",
                "基础治疗"
            };
            Init(basedeck);

            BattleManager.Instance.BattleStart();
        }
        else
        {
            Deck = new();
            foreach(var name in Pack.Cards)
            {
                Deck.Add(name);
            }

            Level = Pack.Level;
            Coin = Pack.Coin;

            if (IsBattle) BattleManager.Instance.BattleStart();
            else
            {
                BattleManager.Instance.SetActive(false);
                ShopManager.Instance.Open();//展示商店
                ShopManager.Instance.Init();
            }

            foreach(var name in Pack.EchoList)
            {
                BattleManager.Instance.Echo(CardData.Cloneby(name));
            }

            Pack = null;
        }
    }

    private void Update()
    {
        CoinText.text = Coin.ToString();
        LevelText.text = $"第{Level}次考验";
    }

    //初始化
    public void Init(CardList basedeck)//basecard表示初始卡组
    {
        Deck = basedeck;
        Level = 0;
    }

    public void SaveGame()
    {
        GameSave.Instance.data.GamePack_ = new()
        {
            Cards = Deck.GetNameList(),
            IsBattle = IsBattle,
            Level = Level,
            Coin = Coin,
            EchoList = BattleManager.Instance.EchoList_.GetNameList(),
        };
        GameSave.Instance.SaveData();
    }

    //返回主菜单
    public void ReturntoMainPage()
    {
        FadeEvent.Instance.Fadeto("EntranceScene");
    }

    public void ShowResult(bool win)
    {
        ResultCanva.DOFade(1f, 0.2f);
        ResultCanva.interactable = true;
        ResultCanva.blocksRaycasts = true;
        WinImage.enabled = win;
        FailImage.enabled = !win;

        DOTween.To(() => 0, x => { }, 0, 0.5f).OnComplete(() =>
        {
            GameSave.Instance.data.GamePack_ = null;//移除存档
            GameSave.Instance.SaveData();

            FadeEvent.Instance.Fadeto("EntranceScene");
        });
    }
}