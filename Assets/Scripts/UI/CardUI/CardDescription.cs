using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Card))]
public class CardDescription : MonoBehaviour
{
    private Card Card;//绑定的卡牌
    private CardData Data => Card.CardData;

    [SerializeField]
    private TextMeshProUGUI Name;
    [SerializeField]
    private TextMeshProUGUI Description;
    [SerializeField]
    private TextMeshProUGUI Strength;

    [SerializeField]
    private Canvas DescriptionCanvas;

    private void Awake()
    {
        Card = GetComponent<Card>();

        SetActive(false);
    }

    private void Start()
    {
        Name.text = Data.Name_;

        string s = Data.Description_;
        int l = 0;
        while (l < s.Length) 
        {
            if (s[l++] == '$')
            {
                string keyword = "";
                int r = l - 1;
                while (r < s.Length && s[++r] != '$')
                {
                    if (s[r] < '0' || s[r] > '9') keyword += s[r];
                }
                AbilityPack ap = AbilityDictionary.Find(keyword);
                if (ap != null)
                {
                    Description.text += $"<color={ColorUtility.ToHtmlStringRGB(ap.Color)}>{s[l..r]}</color>";
                    //生成能力描述
                }
                l = r;
            }
            else Description.text += s[l];
        }

        Strength.text = Data.Strength_.ToString();
    }

    public void SetActive(bool b)
    {
        DescriptionCanvas.enabled = b;

        if (b)
        {
            if(transform.position.x >= 4) DescriptionCanvas.GetComponent<RectTransform>().anchoredPosition = new(-390, 0);
            else DescriptionCanvas.GetComponent<RectTransform>().anchoredPosition = new(10, 0);
        }
    }
}