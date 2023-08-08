using System.Collections;
using System.Collections.Generic;
using Hara.GUI;
using UnityEngine;
using UnityEngine.UI;

public class PopupPause : PopupBase
{
    [SerializeField] private Button continueBtn;
    [SerializeField] private Button quit;
    [SerializeField] private Button restart;

    public static PopupPause Instance;

    [SerializeField] private Button closeBtn;
    
    public static PopupPause Create()
    {
        if (Instance != null)
        {
            Destroy(Instance.gameObject);
            Instance = null;
        }
        
        var go = PopupManager.instance.GetPopup("PopupPause");
        Instance = go.GetComponent<PopupPause>();
        Instance.LoadPopup();
        return Instance;
    }

    private void LoadPopup()
    {
        Time.timeScale = 0;
    }

    protected void Start()
    {
        continueBtn.onClick.AddListener(() => OnCloseBtnClick(true));
        quit.onClick.AddListener(Quit);
        restart.onClick.AddListener(RestartChapter);
        closeBtn.onClick.AddListener(() => OnCloseBtnClick(false));
    }


    [GUIDelegate]
    private void Quit()
    {
        OnCloseBtnClick(true);
        if (GameClient.instance)
        {
            GameClient.instance.BackToMenu();
        }
    }
    

    [GUIDelegate]
    public override void OnCloseBtnClick(bool playSound = true)
    {
        Time.timeScale = 1;
        base.OnCloseBtnClick(playSound);
    }

    [GUIDelegate]
    private void RestartChapter()
    {
        OnCloseBtnClick(true);
        if (GameClient.instance)
        {
            GameClient.instance.LoadChapter((GameClient.instance.UInfo.GetCurrentChapter() + 1));
        }
    }
}
