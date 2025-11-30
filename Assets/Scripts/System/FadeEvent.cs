using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//管理转移场景的脚本
public class FadeEvent : MonoBehaviour
{
    [SerializeField]
    private Animator Anim;

    private string AimScene;//目标场景（名字）

    public static FadeEvent Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void LoadScene()
    {
        SceneManager.LoadScene(AimScene);
    }

    public void Fadeto(string scene)
    {
        AimScene = scene;
        Anim.SetBool("Appear", true);
    }
}