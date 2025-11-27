// DeckArea.cs
using UnityEngine;
using System.Collections.Generic;

public class DeckArea : CardArea
{
    [Header("牌库设置")]
    [SerializeField] private Vector2 positionRange = new Vector2(10f, 10f);
    [SerializeField] private float rotationRange = 5f;
    [SerializeField] private int defaultCardCount = 30;

    private List<CardInteraction> deckCards = new List<CardInteraction>();

    public override void UpdateLayout()
    {
        // 牌库不需要特殊布局，卡牌堆叠在一起
        for (int i = 0; i < cards.Count; i++)
        {
            if (cards[i] != null)
            {
                cards[i].transform.localPosition = GetStackPosition(i);
                cards[i].transform.localRotation = GetStackRotation(i);
                cards[i].transform.localScale = Vector3.one * 0.8f;
            }
        }
    }

    private Vector3 GetStackPosition(int index)
    {
        // 轻微随机偏移，模拟堆叠效果
        float xOffset = Random.Range(-positionRange.x, positionRange.x);
        float yOffset = Random.Range(-positionRange.y, positionRange.y);
        return new Vector3(xOffset, yOffset, -index * 0.01f);
    }

    private Quaternion GetStackRotation(int index)
    {
        // 轻微随机旋转
        float rotation = Random.Range(-rotationRange, rotationRange);
        return Quaternion.Euler(0, 0, rotation);
    }

    public void InitializeDeck(GameObject cardPrefab, int cardCount)
    {
        ClearDeck();

        for (int i = 0; i < cardCount; i++)
        {
            GameObject cardObj = Instantiate(cardPrefab, transform);
            CardInteraction card = cardObj.GetComponent<CardInteraction>();

            if (card != null)
            {
                // 设置卡牌数据（这里需要根据你的卡牌系统来设置）
                // card.Initialize(cardData);

                AddCard(card);
                deckCards.Add(card);
            }
        }

        UpdateLayout();
    }

    public CardInteraction DrawCard()
    {
        if (deckCards.Count > 0)
        {
            CardInteraction card = deckCards[0];
            deckCards.RemoveAt(0);
            RemoveCard(card);
            return card;
        }
        return null;
    }

    public List<CardInteraction> DrawCards(int count)
    {
        List<CardInteraction> drawnCards = new List<CardInteraction>();
        int actualCount = Mathf.Min(count, deckCards.Count);

        for (int i = 0; i < actualCount; i++)
        {
            CardInteraction card = DrawCard();
            if (card != null)
            {
                drawnCards.Add(card);
            }
        }

        return drawnCards;
    }

    private void ClearDeck()
    {
        foreach (CardInteraction card in deckCards)
        {
            if (card != null)
            {
                Destroy(card.gameObject);
            }
        }
        deckCards.Clear();
        cards.Clear();
    }

    public int GetDeckCount() => deckCards.Count;
}