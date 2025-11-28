using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

public class CommandArea : MonoBehaviour
{
    [Header("布局设置")]
    [SerializeField] private int maxCards = 10;
    [SerializeField] private int cardsPerLayer = 5;
    [SerializeField] private float layerSpacing = 150f;
    [SerializeField] private float cardSpacing = 120f;
    [SerializeField] private Vector2 areaCenterOffset = Vector2.zero;

    [Header("动画设置")]
    [SerializeField] private float moveDuration = 0.3f;
    [SerializeField] private Ease moveEase = Ease.OutCubic;

    [Header("布局方向设置")]
    [SerializeField] private bool isBottomFirstRow = false; // 新增：是否下层为第一行

    private List<Card> commandSequence = new List<Card>();
    private Card currentlyDraggedCard;
    private Dictionary<Card, Tween> activeTweens = new Dictionary<Card, Tween>();
    public List<Card> CommandSequence_ { get => commandSequence; }

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
            card.transform.SetParent(transform);
            card.SetInCommandArea(true);

            // 立即设置位置，不使用动画
            Vector3 targetPosition = GetCardPosition(commandSequence.Count - 1);
            card.transform.localPosition = targetPosition;
            card.SetSequenceIndex(commandSequence.Count - 1);

            //Debug.Log($"卡牌添加到指令区，当前序列位置: {commandSequence.Count - 1}");
        }
    }

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
            return;
        }

        // 确保卡牌的父对象是指令区
        if (card.transform.parent != transform)
        {
            card.transform.SetParent(transform);
        }

        // 计算目标位置
        int targetIndex = CalculateDropIndex(dropPosition);
        int currentIndex = commandSequence.IndexOf(card);

        // 只有当位置真正改变时才进行交换
        if (targetIndex >= 0 && targetIndex < commandSequence.Count && targetIndex != currentIndex)
        {
            // 执行交换而不是插入
            SwapCards(currentIndex, targetIndex);
        }
        else
        {
            // 即使位置没变，也要确保卡牌回到正确位置
            Vector3 targetPosition = GetCardPosition(currentIndex);
            AnimateCardToPosition(card, targetPosition, moveDuration);
        }

        currentlyDraggedCard = null;
    }

    // 交换两个卡牌的位置
    private void SwapCards(int indexA, int indexB)
    {
        if (indexA < 0 || indexB < 0 || indexA >= commandSequence.Count || indexB >= commandSequence.Count)
            return;

        Card cardA = commandSequence[indexA];
        Card cardB = commandSequence[indexB];

        if (cardA == null || cardB == null)
            return;

        // 交换序列中的位置
        commandSequence[indexA] = cardB;
        commandSequence[indexB] = cardA;

        // 获取目标位置
        Vector3 posA = GetCardPosition(indexA);
        Vector3 posB = GetCardPosition(indexB);

        // 创建交换动画序列
        Sequence swapSequence = DOTween.Sequence();

        // 卡牌A移动到卡牌B的位置
        swapSequence.Join(cardA.transform.DOLocalMove(posB, moveDuration).SetEase(moveEase));

        // 卡牌B移动到卡牌A的位置
        swapSequence.Join(cardB.transform.DOLocalMove(posA, moveDuration).SetEase(moveEase));

        // 更新序列号
        cardA.SetSequenceIndex(indexB);
        cardB.SetSequenceIndex(indexA);

        //Debug.Log($"交换卡牌 {cardA.GetCardName()} 和 {cardB.GetCardName()}，位置 {indexA} 和 {indexB}");
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

        // 可以在这里添加拖拽时的预览效果
        if (targetIndex != currentIndex && targetIndex >= 0 && targetIndex < commandSequence.Count)
        {
            // 例如高亮目标位置
            // 或者显示插入位置的预览
        }
    }

    private int CalculateDropIndex(Vector3 dropPosition)
    {
        // 将世界坐标转换为本地坐标
        Vector3 localPos = transform.InverseTransformPoint(dropPosition);

        // 减去中心偏移
        localPos.x -= areaCenterOffset.x;
        localPos.y -= areaCenterOffset.y;

        // 计算行 - 使用更精确的计算方法
        // 将Y坐标转换为行索引，考虑卡牌的高度
        int row = CalculateRowIndex(localPos.y);

        // 计算列
        int col = CalculateColumnIndex(localPos.x);

        // 计算索引
        int index = row * cardsPerLayer + col;

        // 确保索引在有效范围内
        index = Mathf.Clamp(index, 0, commandSequence.Count);

        //Debug.Log($"位置计算: 本地位置({localPos.x:F1}, {localPos.y:F1}) -> 行{row}, 列{col} -> 索引{index}");

        return index;
    }

    private int CalculateRowIndex(float localY)
    {
        // 计算行索引，考虑卡牌的高度
        int totalRows = (maxCards - 1) / cardsPerLayer;
        float topY = (layerSpacing * totalRows) / 2f;
        float bottomY = -topY;

        if (isBottomFirstRow)
        {
            // 第一行在底部，Y值最小
            float relativeY = localY - bottomY;
            int row = Mathf.RoundToInt(relativeY / layerSpacing);
            row = Mathf.Clamp(row, 0, totalRows);
            return row;
        }
        else
        {
            // 第一行在顶部，Y值最大（原有逻辑）
            float relativeY = topY - localY;
            int row = Mathf.RoundToInt(relativeY / layerSpacing);
            row = Mathf.Clamp(row, 0, totalRows);
            return row;
        }
    }

    private int CalculateColumnIndex(float localX)
    {
        // 计算列索引
        // 中间列在X=0，左右对称

        // 计算相对于中间列的偏移
        float relativeX = localX + (cardsPerLayer - 1) * cardSpacing / 2f;

        // 计算列索引
        int col = Mathf.RoundToInt(relativeX / cardSpacing);

        // 确保列在有效范围内
        col = Mathf.Clamp(col, 0, cardsPerLayer - 1);

        return col;
    }

    // 平滑更新布局
    private void SmoothUpdateLayout()
    {
        // 停止所有现有动画
        foreach (var tween in activeTweens.Values)
        {
            if (tween != null && tween.IsActive())
                tween.Kill();
        }
        activeTweens.Clear();

        // 为所有卡牌创建新动画
        for (int i = 0; i < commandSequence.Count; i++)
        {
            Card card = commandSequence[i];
            if (card != null && card != currentlyDraggedCard)
            {
                // 确保卡牌的父对象是指令区
                if (card.transform.parent != transform)
                {
                    card.transform.SetParent(transform);
                }

                Vector3 position = GetCardPosition(i);
                AnimateCardToPosition(card, position, moveDuration);
                card.SetSequenceIndex(i);
            }
        }
    }

    // 动画移动卡牌到指定位置
    private void AnimateCardToPosition(Card card, Vector3 targetPosition, float duration)
    {
        // 停止该卡牌现有的动画
        if (activeTweens.ContainsKey(card))
        {
            if (activeTweens[card] != null && activeTweens[card].IsActive())
            {
                activeTweens[card].Kill();
            }
            activeTweens.Remove(card);
        }

        // 创建新的动画
        Tween tween = card.transform.DOLocalMove(targetPosition, duration)
            .SetEase(moveEase)
            .OnComplete(() =>
            {
                if (activeTweens.ContainsKey(card))
                    activeTweens.Remove(card);
            });

        // 记录动画
        activeTweens[card] = tween;
    }

    private Vector3 GetCardPosition(int sequenceIndex)
    {
        int layer = sequenceIndex / cardsPerLayer;
        int positionInLayer = sequenceIndex % cardsPerLayer;
        int totalRows = (maxCards - 1) / cardsPerLayer;
        float topY = (layerSpacing * totalRows) / 2f;
        float bottomY = -topY;

        float layerY;
        if (isBottomFirstRow)
        {
            // 第一行在底部
            layerY = bottomY + layer * layerSpacing + areaCenterOffset.y;
        }
        else
        {
            // 第一行在顶部（原有逻辑）
            layerY = topY - layer * layerSpacing + areaCenterOffset.y;
        }

        float totalWidth = (cardsPerLayer - 1) * cardSpacing;
        float startX = -totalWidth / 2f + areaCenterOffset.x;
        float x = startX + positionInLayer * cardSpacing;

        return new Vector3(x, layerY, -sequenceIndex * 0.1f);
    }

    // 工具方法
    public List<Card> GetCommandSequence() => new List<Card>(commandSequence);

    public void RemoveCard(Card card)
    {
        commandSequence.Remove(card);

        // 停止该卡牌的动画
        if (activeTweens.ContainsKey(card))
        {
            if (activeTweens[card] != null && activeTweens[card].IsActive())
            {
                activeTweens[card].Kill();
            }
            activeTweens.Remove(card);
        }

        // 立即更新布局，不使用动画
        UpdateLayoutImmediately();
    }

    public void ClearCommandArea()
    {
        // 停止所有动画
        foreach (var tween in activeTweens.Values)
        {
            if (tween != null && tween.IsActive())
                tween.Kill();
        }
        activeTweens.Clear();

        foreach (Card card in commandSequence)
        {
            card.SetInCommandArea(false);
        }
        commandSequence.Clear();
    }

    // 立即更新布局，不使用动画
    private void UpdateLayoutImmediately()
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

                Vector3 position = GetCardPosition(i);
                card.transform.localPosition = position;
                card.transform.localRotation = Quaternion.identity;
                card.transform.localScale = Vector3.one;
                card.SetSequenceIndex(i);
            }
        }
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

    public bool CanAcceptMoreCards()
    {
        return commandSequence.Count < maxCards;
    }

    public int GetAvailableSlots()
    {
        return maxCards - commandSequence.Count;
    }

    // 移动卡牌在序列中的位置（带动画）
    public void MoveCardInSequence(Card card, int newSequenceIndex)
    {
        int currentIndex = commandSequence.IndexOf(card);
        if (currentIndex >= 0 && newSequenceIndex >= 0 && newSequenceIndex < commandSequence.Count)
        {
            commandSequence.RemoveAt(currentIndex);
            commandSequence.Insert(newSequenceIndex, card);
            SmoothUpdateLayout();
        }
    }

    // 新增：设置第一行在下方还是上方
    public void SetBottomFirstRow(bool isBottomFirst)
    {
        isBottomFirstRow = isBottomFirst;
        UpdateLayoutImmediately();
    }

    void OnDestroy()
    {
        // 清理所有动画
        foreach (var tween in activeTweens.Values)
        {
            if (tween != null && tween.IsActive())
                tween.Kill();
        }
        activeTweens.Clear();
    }
}