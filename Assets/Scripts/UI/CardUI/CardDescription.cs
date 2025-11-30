using System;
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

    private int AbilityCount = 0;

    [SerializeField]
    private Transform AbilityTextField;//能力解释文本区域
    [SerializeField]
    private GameObject AbilityTextOb;//能力解释物体

    private GameObject ob;

    private void Awake()
    {
        Card = GetComponent<Card>();

        SetActive(false);
    }

    private void Start()
    {
        Name.text = Data.Name_;
        Description.text = "";

        string s = Data.Description_;

        AbilityCount = 0;
        AddAbility(Ability.无声, () => Data.IsSilent);
        if (AbilityCount > 0) Description.text += "\n";

        //读取数据中的描述文本
        int l = 0;
        while (l < s.Length) 
        {
            if (s[l++] == '$')
            {
                string keyword = "";
                int r = l - 1;
                while (++r < s.Length && s[r] != '$')
                {
                    if (s[r] < '0' || s[r] > '9') keyword += s[r];
                }
                AbilityPack ap = AbilityDictionary.Find(keyword);
                if (ap != null)
                {
                    AbilityCount++;

                    Description.text += $"<color=#{ColorUtility.ToHtmlStringRGB(ap.Color)}>{s[l..r]}</color>";

                    //生成能力描述
                    ob = Instantiate(AbilityTextOb, AbilityTextField);
                    ob.GetComponent<AbilityText>().SetData(ap);
                }
                l = r + 1;
            }
            else Description.text += s[l];
        }

        Strength.text = Data.Strength_.ToString();
    }

    private void AddAbility(Ability ability, Func<bool> judge)
    {
        if (judge())
        {
            if (AbilityCount > 0) Description.text += " ";
            AbilityPack ap = AbilityDictionary.Find(ability);
            if (ap != null)
            {
                AbilityCount++;

                Description.text += $"<color=#{ColorUtility.ToHtmlStringRGB(ap.Color)}>{ability}</color>";

                //生成能力描述
                ob = Instantiate(AbilityTextOb, AbilityTextField);
                ob.GetComponent<AbilityText>().SetData(ap);
            }
        }
    }

    public void SetActive(bool b)
    {
        DescriptionCanvas.enabled = b;

        if (b)
        {
            if(transform.position.x >= 4)
            {
                DescriptionCanvas.GetComponent<RectTransform>().anchoredPosition = new(-390, 0);
                AbilityTextField.GetComponent<RectTransform>().anchoredPosition = new(-510, 0);
            }
            else
            {
                DescriptionCanvas.GetComponent<RectTransform>().anchoredPosition = new(10, 0);
                AbilityTextField.GetComponent<RectTransform>().anchoredPosition = new(10, 0);
            }
        }
    }
}