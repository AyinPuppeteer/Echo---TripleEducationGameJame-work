using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//管理主菜单的脚本
public class MainMenuManager : MonoBehaviour
{
    [SerializeField]
    private Button ContinueButton;//“继续”按钮

    private void Start()
    {
        if(GameSave.Instance.data.GamePack_ == null) ContinueButton.gameObject.SetActive(false);
    }

    public void StartGame()
    {
        FadeEvent.Instance.Fadeto("GameScene");
    }

    public void Continue()
    {
        GameManager.Pack = GameSave.Instance.data.GamePack_;
        FadeEvent.Instance.Fadeto("GameScene");
    }

    //退出游戏
    public void QuitGame()
    {
        Application.Quit();
    }
}