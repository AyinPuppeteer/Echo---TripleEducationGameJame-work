using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//¿¨ÅÆ³ÂÁÐ
public class CardsDisplay : MonoBehaviour
{
    //¿¨ÅÆÎïÌå
    [SerializeField]
    private GameObject CardOb;
    //¿¨Æ¬ÇøÓò
    [SerializeField]
    private GameObject CardField;

    [SerializeField]
    private Animator Anim;

    private readonly List<Card> Cards = new();

    private GameObject ob;

    public void CreateCards(CardList cards)
    {
        foreach(var card in Cards)
        {
            Destroy(card.gameObject);
        }
        Cards.Clear();

        foreach(var data in cards)
        {
            CreateCard(data);
        }
        CardField.GetComponent<RectTransform>().sizeDelta = new(CardField.GetComponent<RectTransform>().sizeDelta.x, cards.Count / 5 * 130 + 130);

        Anim.SetBool("IsShow", true);
    }

    private void CreateCard(CardData data)
    {
        ob = Instantiate(CardOb, CardField.transform);
        Card card = ob.GetComponent<Card>();
        card.Initialize(data);
        card.SetDraggable(false);
        Cards.Add(card);
    }

    public void Hide()
    {
        Anim.SetBool("IsShow", false);
    }
}