using System;
using System.Collections;
using System.Collections.Generic;
using Hara.GUI;
using Hiker.Networks.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupEndGame : PopupBase
{
    public static PopupEndGame instance;

    [Header("SET UP VITCORY")]
    [SerializeField] private GameObject victory;
    [SerializeField] private  ActiveObj[] stars;
    [SerializeField] private  TextMeshProUGUI victoryGold;
    [SerializeField] private Button victoryMenu;
    [SerializeField] private Button victoryReset;
    [SerializeField] private Button victoryNext;
    
    [Header("Set up for Defeat")]
    [SerializeField] private GameObject defeated;
    [SerializeField] private  TextMeshProUGUI defeatGold;
    [SerializeField] private Button defeatMenu;
    [SerializeField] private Button defeatReset;
    
    private ResultChapter resultChapter;
    private int star;
    private void Start()
    {
        victoryMenu.onClick.AddListener(BackToMenu);
        defeatMenu.onClick.AddListener(BackToMenu);
        victoryNext.onClick.AddListener(Next);

        victoryReset.onClick.AddListener(ResetLevel);
        defeatReset.onClick.AddListener(ResetLevel);
        
    }
    public static PopupEndGame Create(ResultChapter resultBattle, int star)
    {
        if (instance)
        {
            Destroy(instance.gameObject);
            instance = null;
        }
        
        var go = PopupManager.instance.GetPopup("PopupEndGame");
        instance = go.GetComponent<PopupEndGame>();
        instance.Init(resultBattle, star);
        return instance;
    }

    private void Init(ResultChapter resultBattle, int star)
    {
        resultChapter = resultBattle;
        this.star = star;

        if (QuanLyManChoi.Instance)
        {
            if (QuanLyManChoi.Instance.gameType == GAMETYPE.COOP)
            {
                defeatReset.gameObject.SetActive(false);
                victoryReset.gameObject.SetActive(false);
                victoryNext.gameObject.SetActive(false);
            }
            else
            {
                defeatReset.gameObject.SetActive(true);
                victoryReset.gameObject.SetActive(true);
                victoryNext.gameObject.SetActive(true);
            }
        }
        if (resultBattle.result == ResultBattle.WIN)
        {
            SetUpVictory();
        }
        else
        {
            SetUpDefeat();
        }
    }

    private void SetUpVictory()
    {
        victory.SetActive(true);
        defeated.SetActive(false);

        for (int i = 0; i < 3; i++)
        {
            stars[i].ShowDeactive();
        }

        for (int i = 0; i < star; i++)
        {
            stars[i].ShowActive();
        }

        victoryGold.text = resultChapter.goldCollected.ToString();
    }
    
    private void SetUpDefeat()
    {
        victory.SetActive(false);
        defeated.SetActive(true);
        defeatGold.text = resultChapter.goldCollected.ToString();
    }

    private void BackToMenu()
    {
        GameClient.instance.BackToMenu();
        OnCloseBtnClick(true);
    }

    private void ResetLevel()
    {
        GameClient.instance.LoadChapter(resultChapter.chapterIdx + 1);
        OnCloseBtnClick(true);
    }

    private void Next()
    {
        GameClient.instance.LoadChapter(resultChapter.chapterIdx + 2);
        OnCloseBtnClick(true);
    }
}
