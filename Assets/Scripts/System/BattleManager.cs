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
        Canva.alpha = b ? 1 : 0;
        Canva.interactable = b;
        Canva.blocksRaycasts = b;
    }

    private Individual Player, Mirror;//玩家和镜像单位
    public Individual Player_ { get => Player; }
    public Individual Mirror_ { get => Player_; }

    private CardList Deck;//卡组

    [SerializeField]
    private Transform CardField_Player, CardField_Mirror;
    [SerializeField]
    private GameObject CardOb;//卡片物体

    public static BattleManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        SetActive(false);
    }

    //战斗开始
    public void BattleStart()
    {
        SetActive(true);

        Player = new(20)
        {
            WhenDead = Failed
        };
        Mirror = new(20)
        {
            WhenDead = Win
        };
        Deck = GameManager.Instance.Deck_.Clone();

        TurnStart();
    }

    //回合开始
    public void TurnStart()
    {
        for(int i = 1; i <= 10; i++)
        {
            DOTween.To(() => 0, x => { }, 0, 0.1f).OnComplete(() =>
            {
                Instantiate(CardOb, CardField_Player);//生成卡片
                //让卡片从卡组位置移动到对应位置并缩放
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

    }
    //战斗失败
    public void Failed()
    {

    }
}