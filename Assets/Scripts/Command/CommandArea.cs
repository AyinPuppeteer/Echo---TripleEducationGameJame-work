using UnityEngine;
using System.Collections.Generic;

public class CommandArea : MonoBehaviour
{
    [Header("布局设置")]
    [SerializeField] private int maxCards = 10;
    [SerializeField] private int cardsPerLayer = 5;
    [SerializeField] private float layerSpacing = 150f;
    [SerializeField] private float cardSpacing = 120f;

    private List<Card> commandSequence = new List<Card>();
    private Card currentlyDraggedCard;

    // 添加缺失的 AddCard 方法
    public void AddCard(Card card)
    {
        if (commandSequence.Count >= maxCards)
        {
            Debug.LogWarning("指令区已满，无法添加更多卡牌");
            card.ReturnToOriginalPosition();
            return;
        }

        if (!commandSequence.Contains(card))
        {
            commandSequence.Add(card);
            card.transform.SetParent(transform); // 确保父对象是指令区
            card.SetInCommandArea(true);
            UpdateLayout();

            Debug.Log($"卡牌添加到指令区，当前序列位置: {commandSequence.Count - 1}");
        }
    }

    // 添加缺失的 GetCurrentSequence 方法
    public List<Card> GetCurrentSequence()
    {
        return new List<Card>(commandSequence);
    }

    public void HandleCardDrop(Card card, Vector3 dropPosition)
    {
        // 如果卡牌不在指令区，先添加它
        if (!commandSequence.Contains(card))
        {
            AddCard(card);
            return; // 新卡牌添加到末尾，不需要重新排序
        }

        // 确保卡牌的父对象是指令区
        if (card.transform.parent != transform)
        {
            card.transform.SetParent(transform);
        }

        // 计算新的序列位置
        int newIndex = CalculateDropIndex(dropPosition);
        int currentIndex = commandSequence.IndexOf(card);

        // 只有当位置真正改变时才进行移动
        if (newIndex >= 0 && newIndex < commandSequence.Count && newIndex != currentIndex)
        {
            // 移动卡牌到新位置
            commandSequence.RemoveAt(currentIndex);
            commandSequence.Insert(newIndex, card);

            Debug.Log($"移动卡牌 {card.GetCardName()} 从位置 {currentIndex} 到 {newIndex}");
        }

        UpdateLayout();
        currentlyDraggedCard = null;
    }

    // 在拖拽过程中处理卡牌位置预览
    public void HandleCardDrag(Card card, Vector3 dragPosition)
    {
        if (!commandSequence.Contains(card))
            return;

        currentlyDraggedCard = card;

        // 计算目标位置
        int targetIndex = CalculateDropIndex(dragPosition);
        int currentIndex = commandSequence.IndexOf(card);

        // 预览位置变化（可选：可以添加视觉反馈）
        if (targetIndex != currentIndex && targetIndex >= 0 && targetIndex < commandSequence.Count)
        {
            // 这里可以添加拖拽时的预览效果
            // 例如高亮目标位置或临时调整布局
        }
    }

    private int CalculateDropIndex(Vector3 dropPosition)
    {
        // 将世界坐标转换为本地坐标
        Vector3 localPos = transform.InverseTransformPoint(dropPosition);

        // 计算行和列
        int row = Mathf.FloorToInt(localPos.y / layerSpacing);
        int col = Mathf.RoundToInt((localPos.x + (cardsPerLayer - 1) * cardSpacing / 2) / cardSpacing);

        // 确保列在有效范围内
        col = Mathf.Clamp(col, 0, cardsPerLayer - 1);

        // 计算索引
        int index = row * cardsPerLayer + col;
        return Mathf.Clamp(index, 0, commandSequence.Count);
    }

    private void UpdateLayout()
    {
        for (int i = 0; i < commandSequence.Count; i++)
        {
            Card card = commandSequence[i];
            if (card != null)
            {
                // 确保卡牌的父对象是指令区
                if (card.transform.parent != transform)
                {
                    card.transform.SetParent(transform);
                }

                // 不更新正在拖拽的卡牌位置
                if (card != currentlyDraggedCard)
                {
                    Vector3 position = GetCardPosition(i);
                    card.transform.localPosition = position;
                    card.transform.localRotation = Quaternion.identity;
                    card.transform.localScale = Vector3.one;
                    card.SetSequenceIndex(i);
                }
            }
        }

        // 确保所有卡牌都启用了交互
        foreach (Card card in commandSequence)
        {
            if (card != null)
            {
                card.SetInteractable(true);
            }
        }
    }

    private Vector3 GetCardPosition(int sequenceIndex)
    {
        int layer = sequenceIndex / cardsPerLayer;
        int positionInLayer = sequenceIndex % cardsPerLayer;

        float layerY = layer * layerSpacing;
        float totalWidth = (cardsPerLayer - 1) * cardSpacing;
        float startX = -totalWidth / 2f;
        float x = startX + positionInLayer * cardSpacing;

        return new Vector3(x, layerY, -sequenceIndex * 0.1f);
    }

    // 工具方法
    public List<Card> GetCommandSequence() => new List<Card>(commandSequence);

    public void RemoveCard(Card card)
    {
        commandSequence.Remove(card);
        UpdateLayout();
    }

    public void ClearCommandArea()
    {
        foreach (Card card in commandSequence)
        {
            card.SetInCommandArea(false);
        }
        commandSequence.Clear();
    }

    [ContextMenu("打印当前序列")]
    public void PrintCurrentSequence()
    {
        Debug.Log("=== 指令区当前序列 ===");
        for (int i = 0; i < commandSequence.Count; i++)
        {
            if (commandSequence[i] != null)
            {
                Debug.Log($"{i}: {commandSequence[i].GetCardName()}");
            }
        }
    }

    // 其他可能用到的方法
    public bool CanAcceptMoreCards()
    {
        return commandSequence.Count < maxCards;
    }

    public int GetAvailableSlots()
    {
        return maxCards - commandSequence.Count;
    }

    // 移动卡牌在序列中的位置
    public void MoveCardInSequence(Card card, int newSequenceIndex)
    {
        int currentIndex = commandSequence.IndexOf(card);
        if (currentIndex >= 0 && newSequenceIndex >= 0 && newSequenceIndex < commandSequence.Count)
        {
            commandSequence.RemoveAt(currentIndex);
            commandSequence.Insert(newSequenceIndex, card);
            UpdateLayout();
        }
    }
}