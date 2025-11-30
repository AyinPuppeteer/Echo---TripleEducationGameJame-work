using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

//管理存档的脚本
//管理游戏存档的脚本
public class GameSave : MonoBehaviour
{
    private SaveData Data;//当前运行的存档
    public SaveData data { get => Data; }

    private const string SaveFileName = "GameSave.save";

    public static GameSave Instance { get; private set; }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);//永续存档类
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        Data = LoadData();
        SaveData();
    }

    #region 存档加载和读取
    public void SaveData()
    {
        // 序列化为JSON
        string json = JsonUtility.ToJson(Data);

        // 构建保存路径
        string path = Path.Combine(Application.persistentDataPath, SaveFileName);

        // 写入文件
        File.WriteAllText(path, json);
        Debug.Log($"成功保存到 {path}");
    }
    private SaveData LoadData()
    {
        string path = Path.Combine(Application.persistentDataPath, SaveFileName);

        if (File.Exists(path))
        {
            // 读取内容
            string json = File.ReadAllText(path);
            Debug.Log($"成功读取到 {path}");
            return JsonUtility.FromJson<SaveData>(json);
        }
        else
        {
            Debug.LogWarning($"存档文件不存在: {path}");
            return new();
        }
    }
    #endregion

}

//存档类
[Serializable]
public class SaveData
{
    private GamePack GamePack;
    public GamePack GamePack_ { get => GamePack; set => GamePack = value; }
}

//游戏进度包
[Serializable]
public class GamePack
{
    public int Level;//当前战斗次数

    public bool IsBattle;//是否正在战斗（不是则说明在商店）

    public int Coin;//货币数量

    public List<string> Cards;//持有的卡牌（名字）
}