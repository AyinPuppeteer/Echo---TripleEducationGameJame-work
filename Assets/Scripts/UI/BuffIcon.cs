using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuffIcon : MonoBehaviour
{
    [SerializeField]
    private Image Icon;//Í¼±ê

    [SerializeField]
    private TextMeshProUGUI CountText;//²ãÊý

    private Buff Buff;

    public void Init(Buff buff)
    {
        Buff = buff;
        Icon.sprite = BuffIcon_Creator.GetIcon(buff.Name_);
    }

    private void Update()
    {
        if(Buff.DelTag_)
        {
            Destroy(gameObject);
            return;
        }
        CountText.text = Buff.Count_.ToString();
    }
}