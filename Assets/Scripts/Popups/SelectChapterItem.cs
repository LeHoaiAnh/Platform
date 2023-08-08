using System;
using System.Collections;
using System.Collections.Generic;
using Hiker.Networks.Data;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectChapterItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI indexText;
    [SerializeField] private Button btn;
    [SerializeField] private GameObject activeObj;
    [SerializeField] private GameObject lockObj;
    [SerializeField] private ActiveObj[] star;
    
    private int chapterIdx;

    private void Start()
    {
        btn.onClick.AddListener(LoadChapter);
    }
    
    private void LoadChapter()
    {
        if (!lockObj.activeInHierarchy)
        {
            GameClient.instance.LoadChapter(chapterIdx + 1);
        }
        else
        {
            PopupMessage.Create(MessagePopupType.TEXT, "Clear previous chapter to play");
        }
    }

    public void SetChapter(int idx)
    {
        chapterIdx = idx;
        
        if (GameClient.instance == null ||
            GameClient.instance.UInfo == null)
        {
            return;
        }

        ChapterData chapterData = GameClient.instance.UInfo.GetChapterData(idx);
        if (chapterData == null)
        {
            lockObj.SetActive(true);
            activeObj.SetActive(false);
            indexText.text = String.Format("{0}", idx + 1);
        }
        else
        {
            activeObj.SetActive(true);
            lockObj.SetActive(false);
            indexText.text = String.Format("{0}", idx + 1);
            
            for (int i = 0; i < 3; i++)
            {
                star[i].ShowDeactive();
            }
            if (chapterData.IsComplete)
            {
                for (int i = 0; i < chapterData.star; i++)
                {
                    star[i].ShowActive();
                }
                
            }
        }
    }
}
