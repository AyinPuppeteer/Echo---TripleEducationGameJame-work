using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//管理商店UI的脚本
public class ShopManager : MonoBehaviour
{
    private GameObject ob;

    public static ShopManager Instance { get; private set; }

    private void Awkae()
    {
        Instance = this;
    }

    public void Init()
    {
        
    }
}