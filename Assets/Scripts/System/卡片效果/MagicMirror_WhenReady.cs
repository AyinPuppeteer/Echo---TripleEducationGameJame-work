using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicMirror_WhenReady : CardEffect_WhenReady
{
    public MagicMirror_WhenReady()
    {
        Description = "¸´ÖÆ×ó²àÄ§·¨";
    }

    public override void OnWork(Card card)
    {
        Card c = card.Last;
        if (c != null) card.Initialize(CardData.Cloneby(c.CardData));
    }
}