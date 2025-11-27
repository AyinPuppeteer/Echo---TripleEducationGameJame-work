using UnityEngine;
using System.Collections.Generic;

public class CommandSequenceManager : MonoBehaviour
{
    [SerializeField] private string commandAreaType = "CommandArea";

    private List<CardInteraction> commandSequence = new List<CardInteraction>();

    public System.Action<List<CardInteraction>> OnSequenceChanged;

    private void Start()
    {
        // 注册所有现有卡牌
        foreach (var card in FindObjectsOfType<CardInteraction>())
        {
            card.OnCardDropped += HandleCardDropped;
        }
    }

    private void HandleCardDropped(CardInteraction card, CardArea area)
    {
        if (area.AreaType == commandAreaType)
        {
            AddToSequence(card);
        }
        else
        {
            RemoveFromSequence(card);
        }
    }

    private void AddToSequence(CardInteraction card)
    {
        if (!commandSequence.Contains(card))
        {
            commandSequence.Add(card);
            UpdateSequenceIndices();
            OnSequenceChanged?.Invoke(commandSequence);
        }
    }

    private void RemoveFromSequence(CardInteraction card)
    {
        commandSequence.Remove(card);
        UpdateSequenceIndices();
        OnSequenceChanged?.Invoke(commandSequence);
    }

    private void UpdateSequenceIndices()
    {
        // 这里可以添加序列号视觉反馈
        for (int i = 0; i < commandSequence.Count; i++)
        {
            // commandSequence[i].SetSequenceIndex(i); // 如果需要显示序列号
        }
    }

    public List<CardInteraction> GetCurrentSequence() => new List<CardInteraction>(commandSequence);
}