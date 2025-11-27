using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardDisplay : MonoBehaviour
{
    [Header("卡牌数据")]
    [SerializeField] private CardData cardData;

    [Header("卡牌视觉")]
    [SerializeField] private Image cardArtImage;
    [SerializeField] private Image cardFrameImage;
    [SerializeField] private TextMeshProUGUI sequenceText; // 序列号显示

    [Header("卡牌属性")]
    [SerializeField] private string cardName;

    public CardData CardData => cardData;

    private void Start()
    {
        UpdateCardEffect();
        UpdateCardAppearance();
    }

    public void Initialize(CardData data)
    {
        cardData = data;
        UpdateCardEffect();
        UpdateCardAppearance();
    }

    private void UpdateCardEffect()
    {
        if (cardData == null) return;
        // 更新卡牌名称
        cardName = cardData.Name_;
    }

    private void UpdateCardAppearance()
    {
        if (cardData == null) return;

        // 只更新卡图
        //if (cardArtImage != null && cardData.CardArt != null)
        //{
        //    cardArtImage.sprite = cardData.CardArt;
        //}
    }

    // 序列号显示
    public void SetSequenceNumber(int sequenceIndex)
    {
        if (sequenceText != null)
        {
            if (sequenceIndex >= 0)
            {
                sequenceText.text = (sequenceIndex + 1).ToString();
                sequenceText.gameObject.SetActive(true);
            }
            else
            {
                sequenceText.gameObject.SetActive(false);
            }
        }
    }
}