using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//管理主菜单的脚本
public class MainMenuManager : MonoBehaviour
{
    public void StartGame()
    {
        FadeEvent.Instance.Fadeto("GameScene");
    }

    //退出游戏
    public void QuitGame()
    {
        Application.Quit();
    }
}