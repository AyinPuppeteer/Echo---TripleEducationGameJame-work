// TooltipContent.cs
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class TooltipContent : MonoBehaviour
{
    [Header("Tooltip UI引用")]
    [SerializeField] private TextMeshProUGUI cardNameText;
    [SerializeField] private TextMeshProUGUI manaCostText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI attackText;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI typeLineText;
    [SerializeField] private Image cardArtImage;
    [SerializeField] private Image backgroundImage;

    public void Initialize(CardData cardData)
    {
        if (cardData == null) return;

        // 设置基本信息
        //SetTextSafely(cardNameText, cardData.CardName);
        //SetTextSafely(manaCostText, cardData.ManaCost.ToString());
        //SetTextSafely(descriptionText, ParseKeywords(cardData.Description));

        // 设置类型行
        //string typeLine = GetCardType(cardData);
        //SetTextSafely(typeLineText, typeLine);

        // 设置战斗属性（如果是单位牌）
        //bool isUnit = cardData.Attack > 0 || cardData.Health > 0;
        //SetTextSafely(attackText, isUnit ? cardData.Attack.ToString() : "");
        //SetTextSafely(healthText, isUnit ? cardData.Health.ToString() : "");

        // 激活/禁用统计区域
        if (attackText != null && attackText.transform.parent != null)
        {
            //attackText.transform.parent.gameObject.SetActive(isUnit);
        }

        // 设置卡图
        //if (cardArtImage != null && cardData.CardArt != null)
        //{
        //    cardArtImage.sprite = cardData.CardArt;
        //}

        // 根据卡牌类型设置样式
        //ApplyCardTypeStyling(cardData, typeLine);
    }

    private void SetTextSafely(TextMeshProUGUI textComponent, string value)
    {
        if (textComponent != null)
        {
            textComponent.text = value;
        }
    }

    private string ParseKeywords(string description)
    {
        // 关键词高亮逻辑
        // 例如：将"冲锋"替换为"<color=#FFD700>冲锋</color>"
        string[] keywords = { "冲锋", "嘲讽", "圣盾", "剧毒", "亡语", "战吼" };

        foreach (string keyword in keywords)
        {
            if (description.Contains(keyword))
            {
                description = description.Replace(keyword, $"<color=#FFD700><b>{keyword}</b></color>");
            }
        }

        return description;
    }

    //private string GetCardType(CardData cardData)
    //{
    //    if (cardData.Attack > 0 || cardData.Health > 0)
    //    {
    //        return $"随从 - {GetManaCostText(cardData.ManaCost)}费";
    //    }
    //    else
    //    {
    //        return $"法术 - {GetManaCostText(cardData.ManaCost)}费";
    //    }
    //}

    private string GetManaCostText(int manaCost)
    {
        return manaCost.ToString();
    }

    private void ApplyCardTypeStyling(CardData cardData, string typeLine)
    {
        if (backgroundImage == null) return;

        // 根据卡牌类型设置背景颜色
        //if (cardData.Attack > 0 || cardData.Health > 0)
        //{
        //    // 随从卡 - 蓝色系
        //    backgroundImage.color = new Color(0.1f, 0.3f, 0.6f, 0.95f);
        //}
        //else
        //{
        //    // 法术卡 - 紫色系
        //    backgroundImage.color = new Color(0.4f, 0.1f, 0.6f, 0.95f);
        //}
    }
}