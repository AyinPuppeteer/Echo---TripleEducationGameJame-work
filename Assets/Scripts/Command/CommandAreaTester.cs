using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CommandAreaTester : MonoBehaviour
{
    [Header("测试设置")]
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private CommandArea commandArea;
    [SerializeField] private int testCardCount = 10;

    public void GenerateTestCards()
    {
        if (cardPrefab == null || commandArea == null)
        {
            Debug.LogError("请设置卡牌预制体和指令区引用");
            return;
        }

        // 清空指令区
        commandArea.ClearCommandArea();

        // 生成测试卡牌到指令区
        for (int i = 0; i < testCardCount; i++)
        {
            CreateTestCard(i);
        }

        Debug.Log($"已生成 {testCardCount} 张测试卡牌到指令区，可以开始拖拽排序测试");
    }

    private void CreateTestCard(int index)
    {
        GameObject cardObj = Instantiate(cardPrefab, commandArea.transform);

        // 获取卡牌组件
        Card card = cardObj.GetComponent<Card>();
        if (card != null)
        {
            // 使用CardData.Cloneby方法创建卡牌数据
            /*
            string[] testCardNames = { "攻击卡", "防御卡", "治疗卡", "特殊卡" };
            string cardName = testCardNames[index % testCardNames.Length] + " " + (index + 1);
            */

            CardData cardData = CardData.Cloneby("基础攻击");
            if (cardData != null)
            {
                card.Initialize(cardData);
            }

            // 添加到指令区
            commandArea.AddCard(card);
        }
    }

    // 在编辑器中调用
    [ContextMenu("生成测试卡牌")]
    private void GenerateTestCardsEditor()
    {
        GenerateTestCards();
    }

    [ContextMenu("打印当前序列")]
    public void PrintSequence()
    {
        if (commandArea != null)
        {
            commandArea.PrintCurrentSequence();
        }
    }

    // 执行指令序列
    [ContextMenu("执行序列")]
    public void ExecuteSequence()
    {
        if (commandArea != null)
        {
            var sequence = commandArea.GetCurrentSequence();
            Debug.Log($"开始执行指令序列，共 {sequence.Count} 张卡牌");

            StartCoroutine(ExecuteSequenceCoroutine(sequence));
        }
    }

    private IEnumerator ExecuteSequenceCoroutine(List<Card> sequence)
    {
        for (int i = 0; i < sequence.Count; i++)
        {
            Card card = sequence[i];
            if (card != null && card.CardData != null)
            {
                Debug.Log($"执行第 {i + 1} 张卡: {card.CardData.Name_}");

                // 这里可以添加卡牌效果执行逻辑
                // 例如: card.CardData.WhenPlay(player, enemy);

                yield return new WaitForSeconds(0.5f);
            }
        }

        Debug.Log("指令序列执行完成!");

        // 执行完成后清空指令区
        commandArea.ClearCommandArea();
    }
}