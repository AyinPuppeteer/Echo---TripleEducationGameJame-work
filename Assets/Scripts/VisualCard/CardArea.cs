using UnityEngine;
using System.Collections.Generic;

public abstract class CardArea : MonoBehaviour
{
    [SerializeField] protected string areaType;
    [SerializeField] protected int maxCards = 10;

    protected List<CardInteraction> cards = new List<CardInteraction>();

    public string AreaType => areaType;
    public int CardCount => cards.Count;
    public bool CanAcceptCard(CardInteraction card) => cards.Count < maxCards;

    public virtual void AddCard(CardInteraction card)
    {
        if (!cards.Contains(card))
        {
            cards.Add(card);
            card.transform.SetParent(transform);
            card.SetCurrentArea(this);
            UpdateLayout();
        }
    }

    public virtual void RemoveCard(CardInteraction card)
    {
        cards.Remove(card);
        UpdateLayout();
    }

    public abstract void UpdateLayout();

    protected Vector3 GetLinearPosition(int index, int total, float spacing = 120f)
    {
        float width = (total - 1) * spacing;
        return new Vector3(-width / 2 + index * spacing, 0, -index * 0.1f);
    }
}