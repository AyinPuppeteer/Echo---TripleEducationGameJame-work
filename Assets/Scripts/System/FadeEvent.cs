using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//管理转移场景的脚本
public class FadeEvent : MonoBehaviour
{
    public static FadeEvent Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void Fadeto(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}