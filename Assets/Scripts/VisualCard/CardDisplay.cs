using UnityEngine;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{
    [Header("卡牌数据")]
    [SerializeField] private CardData cardData;

    [Header("卡牌视觉")]
    [SerializeField] private Image cardArtImage;
    [SerializeField] private Image cardFrameImage; // 可选的卡牌边框

    public CardData CardData => cardData;

    private void Start()
    {
        UpdateCardAppearance();
    }

    public void Initialize(CardData data)
    {
        cardData = data;
        UpdateCardAppearance();
    }

    private void UpdateCardAppearance()
    {
        if (cardData == null) return;

        // 只更新卡图
        //if (cardArtImage != null && cardData.CardArt != null)
        //{
        //    cardArtImage.sprite = cardData.CardArt;
        //}

        // 可选：根据卡牌类型设置不同的边框
        //UpdateCardFrame();
    }

    //private void UpdateCardFrame()
    //{
    //    if (cardFrameImage == null) return;

    //    // 根据卡牌类型设置不同颜色的边框
    //    // 例如：随从-蓝色边框，法术-紫色边框，装备-橙色边框
    //    if (cardData.Attack > 0 || cardData.Health > 0)
    //    {
    //        // 随从卡
    //        cardFrameImage.color = new Color(0.2f, 0.4f, 0.8f, 1f);
    //    }
    //    else
    //    {
    //        // 法术卡
    //        cardFrameImage.color = new Color(0.6f, 0.2f, 0.8f, 1f);
    //    }
    //}

    // 可选：卡牌状态变化时的视觉反馈
    //public void SetHighlighted(bool highlighted)
    //{
    //    if (cardFrameImage != null)
    //    {
    //        cardFrameImage.color = highlighted ?
    //            new Color(1f, 0.8f, 0.2f, 1f) : // 高亮颜色
    //            GetDefaultFrameColor(); // 恢复默认颜色
    //    }
    //}

    //private Color GetDefaultFrameColor()
    //{
    //    if (cardData.Attack > 0 || cardData.Health > 0)
    //    {
    //        return new Color(0.2f, 0.4f, 0.8f, 1f);
    //    }
    //    else
    //    {
    //        return new Color(0.6f, 0.2f, 0.8f, 1f);
    //    }
    //}
}