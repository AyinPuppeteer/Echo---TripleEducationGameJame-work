// CommandArea.cs
using UnityEngine;
using System.Collections.Generic;

public class CommandArea : CardArea
{
    [Header("指令区布局设置")]
    [SerializeField] private int cardsPerLayer = 5;
    [SerializeField] private float layerSpacing = 150f;
    [SerializeField] private float cardSpacing = 120f;

    // 序列管理
    private List<CardInteraction> sequence = new List<CardInteraction>();

    public override void UpdateLayout()
    {
        UpdateTwoLayerLayoutWithSequence();
    }

    private void UpdateTwoLayerLayoutWithSequence()
    {
        for (int i = 0; i < sequence.Count; i++)
        {
            if (sequence[i] != null)
            {
                Vector3 position = GetSequencePosition(i);
                sequence[i].transform.localPosition = position;
                sequence[i].transform.localRotation = Quaternion.identity;
                sequence[i].transform.localScale = Vector3.one;

                // 更新卡牌的序列号显示
                UpdateCardSequenceDisplay(sequence[i], i);
            }
        }
    }

    private Vector3 GetSequencePosition(int sequenceIndex)
    {
        // 确定层和位置
        int layer = sequenceIndex / cardsPerLayer;
        int positionInLayer = sequenceIndex % cardsPerLayer;

        float layerY = layer * layerSpacing;

        // 计算水平位置
        float totalWidth = (cardsPerLayer - 1) * cardSpacing;
        float startX = -totalWidth / 2f;
        float x = startX + positionInLayer * cardSpacing;

        return new Vector3(x, layerY, -sequenceIndex * 0.1f);
    }

    private void UpdateCardSequenceDisplay(CardInteraction card, int sequenceIndex)
    {
        // 这里可以调用卡牌的显示方法来更新序列号
        // 例如：card.SetSequenceNumber(sequenceIndex);

        // 临时调试显示
        Debug.Log($"卡牌 {card.GetCardName()} 序列号: {sequenceIndex}");
    }

    public override void AddCard(CardInteraction card)
    {
        if (sequence.Count >= maxCards)
        {
            Debug.LogWarning("指令区已满，无法添加更多卡牌");
            return;
        }

        // 添加到序列末尾
        sequence.Add(card);
        cards.Add(card);
        card.transform.SetParent(transform);
        card.SetCurrentArea(this);

        UpdateLayout();

        Debug.Log($"卡牌添加到指令区，当前序列位置: {sequence.Count - 1}");
    }

    public override void RemoveCard(CardInteraction card)
    {
        int sequenceIndex = sequence.IndexOf(card);
        if (sequenceIndex >= 0)
        {
            sequence.RemoveAt(sequenceIndex);
            cards.Remove(card);

            // 重新排列剩余卡牌的序列
            ReorderSequence();

            UpdateLayout();

            Debug.Log($"卡牌从指令区移除，原序列位置: {sequenceIndex}");
        }
    }

    private void ReorderSequence()
    {
        // 序列已经自动更新，只需要更新显示
        for (int i = 0; i < sequence.Count; i++)
        {
            UpdateCardSequenceDisplay(sequence[i], i);
        }
    }

    // 移动卡牌在序列中的位置
    public void MoveCardInSequence(CardInteraction card, int newSequenceIndex)
    {
        int currentIndex = sequence.IndexOf(card);
        if (currentIndex >= 0 && newSequenceIndex >= 0 && newSequenceIndex < sequence.Count)
        {
            sequence.RemoveAt(currentIndex);
            sequence.Insert(newSequenceIndex, card);

            UpdateLayout();

            Debug.Log($"移动卡牌 {card.GetCardName()} 从位置 {currentIndex} 到 {newSequenceIndex}");
        }
    }

    // 交换两个卡牌的位置
    public void SwapCardsInSequence(int indexA, int indexB)
    {
        if (indexA >= 0 && indexB >= 0 && indexA < sequence.Count && indexB < sequence.Count)
        {
            CardInteraction temp = sequence[indexA];
            sequence[indexA] = sequence[indexB];
            sequence[indexB] = temp;

            UpdateLayout();

            Debug.Log($"交换卡牌位置 {indexA} 和 {indexB}");
        }
    }

    // 获取当前序列
    public List<CardInteraction> GetCurrentSequence()
    {
        return new List<CardInteraction>(sequence);
    }

    // 获取卡牌的序列号
    public int GetCardSequenceIndex(CardInteraction card)
    {
        return sequence.IndexOf(card);
    }

    // 根据序列号获取卡牌
    public CardInteraction GetCardAtSequenceIndex(int index)
    {
        if (index >= 0 && index < sequence.Count)
        {
            return sequence[index];
        }
        return null;
    }

    public bool CanAcceptMoreCards()
    {
        return sequence.Count < maxCards;
    }

    public int GetAvailableSlots()
    {
        return maxCards - sequence.Count;
    }

    // 清空指令区
    [ContextMenu("清空指令区")]
    public void ClearCommandArea()
    {
        // 创建临时列表避免修改时迭代错误
        List<CardInteraction> cardsToRemove = new List<CardInteraction>(sequence);

        foreach (CardInteraction card in cardsToRemove)
        {
            RemoveCard(card);
        }

        sequence.Clear();
        cards.Clear();
        Debug.Log("指令区已清空");
    }

    // 调试：打印当前序列
    [ContextMenu("打印当前序列")]
    public void PrintCurrentSequence()
    {
        Debug.Log("=== 指令区当前序列 ===");
        for (int i = 0; i < sequence.Count; i++)
        {
            if (sequence[i] != null)
            {
                Debug.Log($"{i}: {sequence[i].GetCardName()}");
            }
            else
            {
                Debug.Log($"{i}: [空]");
            }
        }
    }
}