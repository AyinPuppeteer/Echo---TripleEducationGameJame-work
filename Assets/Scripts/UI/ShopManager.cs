using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//管理商店UI的脚本
public class ShopManager : MonoBehaviour
{
    public CanvasGroup Canva;
    public void SetActive(bool b)
    {
        Canva.alpha = b ? 1 : 0;
        Canva.interactable = b;
        Canva.blocksRaycasts = b;
    }

    private GameObject ob;

    public static ShopManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        SetActive(false);
    }

    public void Init()
    {
        
    }
}