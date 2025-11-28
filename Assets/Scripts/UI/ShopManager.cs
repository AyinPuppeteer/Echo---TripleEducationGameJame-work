using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//管理商店UI的脚本
public class ShopManager : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup Canva;

    [SerializeField]
    private Animator Anim;

    [SerializeField]
    private Image BlackImage;

    private GameObject ob;

    public static ShopManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        Close();
        SetActive(-1);
    }

    public void Init()
    {
        
    }

    public void Open()
    {
        Anim.SetBool("IsShow", true);
    }
    public void Close()
    {
        Anim.SetBool("IsShow", false);
    }

    private void SetActive(int t)
    {
        Canva.interactable = t == 1;
        Canva.blocksRaycasts = t == 1;
    }
}