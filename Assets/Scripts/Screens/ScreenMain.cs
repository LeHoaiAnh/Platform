using System;
using System.Collections;
using System.Collections.Generic;
using Hara.GUI;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScreenMain : ScreenBase
{
    public static ScreenMain Instance;
    [Header("Buttons")] [SerializeField]
    private Button inventoryBtn;
    [SerializeField] private Button shopBtn;
    [SerializeField] private Button forgeBtn;
    [SerializeField] private Button settingBtn;
    
    [SerializeField]private Button playBtn;
    [SerializeField]private Button selectLevelBtn;
    [SerializeField]private Button pvpBtn;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        playBtn.onClick.AddListener(Play);
        inventoryBtn.onClick.AddListener(OpenInventory);
        selectLevelBtn.onClick.AddListener(SelectLevel);
        shopBtn.onClick.AddListener(OpenShop);
        forgeBtn.onClick.AddListener(OpenForge);
        pvpBtn.onClick.AddListener(OpenPvP);
        settingBtn.onClick.AddListener(Setting);
    }

    private void Setting()
    {
        
    }

    private void Play()
    {
        //GameClient.instance.LoadChapter(GameClient.instance.UInfo.GetHighestChapter() + 1);
        GameClient.instance.LoadCoOp(4);
    }

    private void OpenForge()
    {
        PopupForge.Create();
    }

    private void OpenShop()
    {
        PopupShop.Create();
    }

    private void SelectLevel()
    {
        PopupSelectLevel.Create();
    }

    void OpenInventory()
    {
        PopupInventory.Create();
    }

    void OpenPvP()
    {
        PopupPvP.Create();
    }
}
