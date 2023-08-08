using System;
using System.Collections;
using System.Collections.Generic;
using Hara.GUI;
using UnityEngine;
using UnityEngine.UI;

public class PopupSelectLevel : PopupBase
{
    public static PopupSelectLevel instance;

    [SerializeField] private List<GroupChapter> groupChapters;
    [SerializeField] private Button closeBtn;

    private void Start()
    {
        closeBtn.onClick.AddListener(() => OnCloseBtnClick(true));
    }

    public static PopupSelectLevel Create()
    {
        if (instance)
        {
            Destroy(instance.gameObject);
            instance = null;
        }

        var go = PopupManager.instance.GetPopup("PopupSelectLevel");
        instance = go.GetComponent<PopupSelectLevel>();
        instance.Init();
        return instance;
    }
    
    private void Init()
    {
        if (GameClient.instance == null ||
            GameClient.instance.UInfo == null)
        {
            return;
        }
        
        groupChapters[0].SetGroupChapter(0, 10);
        groupChapters[1].SetGroupChapter(10, 20);
        groupChapters[2].SetGroupChapter(20, 30);
    }

}
