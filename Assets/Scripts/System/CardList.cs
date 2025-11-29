using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//管理卡片列表的脚本（例如牌组、墓地等)
public class CardList : IEnumerable<CardData>
{
    private readonly List<CardData> Cards = new();

    #region 实现Linq类
    public IEnumerator<CardData> GetEnumerator()
    {
        return Cards.GetEnumerator();
    }
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
    #endregion

    public CardData this[int id] => Cards[id];

    public int Count => Cards.Count;

    public void Add(CardData card)
    {
        CardData newcard = CardData.Cloneby(card);
        Cards.Add(newcard);
    }
    public void Add(string name)
    {
        Add(CardData.Cloneby(name));
    }

    public void AddRange(CardList list)
    {
        Cards.AddRange(list);
    }

    public CardList Clone()
    {
        CardList newlist = new();
        foreach(var card in this)
        {
            newlist.Add(card);
        }
        return newlist;
    }

    public CardData Draw()
    {
        int t = Random.Range(0, Cards.Count);
        CardData card = Cards[t];
        Cards.RemoveAt(t);
        return card;
    }

    public void Clear() => Cards.Clear();

    public List<string> GetNameList() => Cards.Select(card => card.Name_).ToList();
}