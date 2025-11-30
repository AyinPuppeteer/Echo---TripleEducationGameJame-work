using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//用于生成Buff图标的脚本
public class BuffIcon_Creator : MonoBehaviour
{
    [SerializeField]
    private GameObject BuffIconOb;//Buff图标物体

    private const string IconRoot = "Images/BuffIcons/";

    private GameObject ob;

    public static BuffIcon_Creator Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public static Sprite GetIcon(string name)
    {
        Sprite icon = Resources.Load<Sprite>(IconRoot + name);
        if (icon == null)
        {
            Debug.LogError($"未找到该Buff的图标：{icon}");
            return null;
        }
        return icon;
    }

    public void CreateBuffIcon(Buff buff, Transform parent)
    {
        ob = Instantiate(BuffIconOb, parent);
        BuffIcon icon = ob.GetComponent<BuffIcon>();
        icon.Init(buff);
    }
}