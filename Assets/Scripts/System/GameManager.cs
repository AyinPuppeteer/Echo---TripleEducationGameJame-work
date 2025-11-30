using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    private int Level;//第几次战斗
    public int Level_ { get => Level; set => Level = value; }

    private bool IsBattle;//是否处于战斗中

    private float Luckiness;//幸运（影响商店刷新物品的质量）
    public float Luckiness_ { get => Luckiness; }

    [SerializeField]
    private TextMeshProUGUI LevelText;
    [SerializeField]
    private TextMeshProUGUI CoinText;

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
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
            Coin = Coin
        };
        GameSave.Instance.SaveData();
    }

    //返回主菜单
    public void ReturntoMainPage()
    {
        FadeEvent.Instance.Fadeto("EntranceScene");
    }
}