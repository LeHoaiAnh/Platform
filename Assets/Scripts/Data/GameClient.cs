using System;
using System.Collections;
using System.Collections.Generic;
using Hara.GUI;
using Hiker.Networks.Data;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-1000)]
public partial class GameClient : HTTPClient
{ 
    public static GameClient instance;

    public PlayerManagement playerManager { get; set; }
    public SpriteCollection spriteCollection { get; private set; }
    public UserInfo UInfo { get; set; }

    public StoreData StoreData { get; set; }
    public string AccessToken { get; set; }
    public long GID;
    public DateTime LastTimeUpdateInfo { get; private set; }
    public long ServerTimeDiffTick { get; private set; }

#if UNITY_EDITOR
    public bool openAllChap;
    public bool TestScene = false;
    public bool hasAllItem;
    public bool notRunServer;
#endif

    public GAMETYPE gameType;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
        StoreData = GetComponentInChildren<StoreData>();
    }
    
    private void OnEnable()
    {
        ConfigManager.LoadConfigs();
        UInfo = CreateNewUser();
    }

    private void Start()
    {
        spriteCollection = Resources.Load<SpriteCollection>("SpritesCollection");
        //if (!TestScene)
        //{
        //}
        //else
        {
            playerManager = PlayerManagement.Instance;
        }
    }

    public UserInfo CreateNewUser()
    {
        UserInfo userInfo = new UserInfo();
        userInfo.gamer = new GamerData();
        userInfo.listInventory = new List<InventoryInfor>();
#if UNITY_EDITOR
        if (openAllChap)
        {
            for (int i = 0; i < ConfigManager.GetTotalChapter(); i++)
            {
                userInfo.AddNewChapter(i);
            }
        }
        else
        {
            userInfo.AddNewChapter(0);
        }

        if (hasAllItem)
        {
            foreach (var VARIABLE in ConfigManager.itemCfg.listItems)
            {
                foreach (var VARIABLE2 in VARIABLE.items)
                {
                    userInfo.UpdateInventory(new ItemInventoryInfor(VARIABLE2.codename, VARIABLE.typeItem), 2);
                }
            }
        }
#else 
        userInfo.AddNewChapter(0);
#endif
        userInfo.SetUpSlotItemEquip();
        return userInfo;
    }

    //chapter start tu 0
    //scene start tu 1
    public void LoadCoOp(int idx)
    {        
        RequestStartChapter(0);
        StartCoroutine(LoadChapterAsc(idx));
    }
    
    //chapter start tu 0
    //scene start tu 1
    public void LoadChapter(int idx)
    {        
        UInfo.SetCurrentChapter(idx - 1);
        RequestStartChapter(idx - 1);
        StartCoroutine(LoadChapterAsc(idx));
    }

    private IEnumerator LoadChapterAsc(int idx)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(idx);
        if (PopupSelectLevel.instance != null && PopupSelectLevel.instance.gameObject.activeInHierarchy)
        {
            PopupSelectLevel.instance.OnCloseBtnClick();
        }

        PopupLoading.Create();

        while (!operation.isDone)
        {
            yield return null;
        }
        
        while (!StoreData.hasUpdateData)
        {
            yield return null;
        }
        PopupLoading.instance.Close();
        playerManager = PlayerManagement.Instance;
        if (Camera.main != null)
        {
            Camera.main.GetComponent<AudioListener>().enabled = true;
        }
    }

    public void LoadPvP(int idx)
    {
        
    }

    public void BackToMenu()
    {
        StartCoroutine(LoadMenuAsc());

    }

    private IEnumerator LoadMenuAsc()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(0);
     
        PopupLoading.Create();

        while (!operation.isDone)
        {
            yield return null;
        }
        GUIManager.Instance.SetScreen("Main");
        PopupLoading.instance.Close();
    }
}
