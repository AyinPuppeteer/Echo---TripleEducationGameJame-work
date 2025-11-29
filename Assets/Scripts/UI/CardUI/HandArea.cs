// HandArea.cs
using UnityEngine;
using System.Collections.Generic;

public class HandArea : MonoBehaviour
{
    [Header("手牌布局设置")]
    [SerializeField] private float arcHeight = 50f;
    [SerializeField] private float maxRotation = 15f;

    private List<Card> handCards = new List<Card>();

    public void AddCard(Card card)
    {
        if (!handCards.Contains(card))
        {
            handCards.Add(card);
            card.transform.SetParent(transform);
            UpdateLayout();
        }
    }

    public void RemoveCard(Card card)
    {
        handCards.Remove(card);
        UpdateLayout();
    }

    public void UpdateLayout()
    {
        for (int i = 0; i < handCards.Count; i++)
        {
            if (handCards[i] != null)
            {
                Vector3 position = GetArcPosition(i, handCards.Count);
                Quaternion rotation = GetArcRotation(i, handCards.Count);

                handCards[i].transform.localPosition = position;
                handCards[i].transform.localRotation = rotation;
                handCards[i].transform.localScale = Vector3.one * 0.8f;

                // 手牌区不显示序列号
                handCards[i].SetInCommandArea(false);
            }
        }
    }

    private Vector3 GetArcPosition(int index, int total)
    {
        if (total <= 1) return Vector3.zero;

        float normalized = (float)index / (total - 1);
        float angle = Mathf.Lerp(-60f, 60f, normalized) * Mathf.Deg2Rad;

        return new Vector3(
            Mathf.Sin(angle) * 200f,
            Mathf.Cos(angle) * arcHeight - arcHeight,
            -index * 0.1f
        );
    }

    private Quaternion GetArcRotation(int index, int total)
    {
        if (total <= 1) return Quaternion.identity;

        float rotation = Mathf.Lerp(-maxRotation, maxRotation, (float)index / (total - 1));
        return Quaternion.Euler(0, 0, rotation);
    }

    public List<Card> GetHandCards() => new List<Card>(handCards);
}