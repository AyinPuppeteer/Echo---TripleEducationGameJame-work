using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//管理商店UI的脚本
public class ShopManager : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup Canva;

    [SerializeField]
    private Animator Anim;

    [SerializeField]
    private Image BlackImage;

    [SerializeField]
    private TextMeshProUGUI CoinText;//货币数量

    [SerializeField]
    private GameObject CardOb;//卡片物体
    private readonly List<Card> Cards = new();//生成的卡片

    [SerializeField]
    private Transform[] CardPos;//卡片位置

    private List<CardData> AllCards;//所有的卡牌

    [SerializeField]
    private Button[] BuyButtons;//购买按钮
    [SerializeField]
    private TextMeshProUGUI[] ButtonsText;//按钮文本

    private GameObject ob;

    public static ShopManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        SetActive(-1);

        AllCards = CardData.ReturnAllCards();
        AllCards = AllCards.Where(card => card.Rarity_ != Rarity.基础).ToList();//剔除基础牌
    }

    public void Init()
    {
        for(int i = 0; i <= 3; i++)
        {
            int t = Random.Range(0, AllCards.Count);
            ob = Instantiate(CardOb, CardPos[i].position, Quaternion.identity, CardPos[i]);
            Card card = ob.GetComponent<Card>();
            card.Initialize(CardData.Cloneby(AllCards[t]));
            Cards.Add(card);

            ButtonsText[i].text = "购买 2$";
            BuyButtons[i].interactable = true;
        }
        for (int i = 4; i <= 6; i++)
        {
            int t = Random.Range(0, GameManager.Instance.Deck_.Count);
            ob = Instantiate(CardOb, CardPos[i].position, Quaternion.identity, CardPos[i]);
            Card card = ob.GetComponent<Card>();
            card.Initialize(GameManager.Instance.Deck_[t]);
            Cards.Add(card);

            ButtonsText[i].text = "删除 2$";
            BuyButtons[i].interactable = true;
        }
    }

    private void Update()
    {
        CoinText.text = GameManager.Instance.Coin_.ToString();
    }

    public void Buy(int t)
    {
        if(GameManager.Instance.Coin_ >= 2)
        {
            GameManager.Instance.DelCoin(2);
            GameManager.Instance.Deck_.Add(Cards[t].CardData);
            ButtonsText[t].text = "已售出";
            BuyButtons[t].interactable = false;
        }
    }
    public void Del(int t)
    {
        if (GameManager.Instance.Coin_ >= 2)
        {
            GameManager.Instance.DelCoin(2);
            GameManager.Instance.Deck_.Remove(Cards[t].CardData);
            ButtonsText[t].text = "已删除";
            BuyButtons[t].interactable = false;
        }
    }

    public void Open()
    {
        Anim.SetBool("IsShow", true);
    }
    public void Close()
    {
        Anim.SetBool("IsShow", false);

        //摧毁生成的卡片
        foreach(Card card in Cards)
        {
            Destroy(card.gameObject);
        }
        Cards.Clear();

        BattleManager.Instance.BattleStart();
    }

    private void SetActive(int t)
    {
        Canva.interactable = t == 1;
        Canva.blocksRaycasts = t == 1;
    }
}