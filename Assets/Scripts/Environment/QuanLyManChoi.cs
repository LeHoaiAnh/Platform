
using System;
using System.Collections;
using System.Collections.Generic;
using Hiker.Networks.Data;
using Photon.Pun;
using UnityEngine;

[Serializable]
public class ObjPos
{
    public TypeObject type;
    public GameObject[] obj;
}

public enum TypeObject
{
    #region Effect Type
    Increase_EXP_Obj,
    Shield_obj,
    Speed_Up_obj,
    #endregion
    
    #region Boss Type
    Boss
    #endregion
}
public enum GAMESTATE
{
    START,
    INGAME,
    ENDGAME,
    OUTBATTLE,
    NONE
}

public enum GAMETYPE
{
    NORMAL,
    COOP
}

public class QuanLyManChoi : MonoBehaviour
{
    static QuanLyManChoi instance = null;
    public int rewardGold { get; set; }
    public float lightIntensity = 1f;
    public float damageMultiplier = 1f;
    public ObjPos[] items;

    public GAMESTATE gameState;
    public GAMETYPE gameType;
    public float TimeBattle { get; private set; }
    private float timeInterval = 1f;
    private float curTimeInterval;
    
    public static QuanLyManChoi Instance {
        get {
            if (instance == null)
                instance = FindObjectOfType<QuanLyManChoi>();
            return instance;
        }
    }
    public void SetGameState(GAMESTATE _state)
    {
        if (_state == gameState)
        {
            return;
        }

        gameState = _state;
    }

    private void Start()
    {
        SetGameState(GAMESTATE.START);
    }

    private void Update()
    {
        if(gameState == GAMESTATE.START)
        {
            OnStart();
        }

        if(gameState == GAMESTATE.INGAME)
        {
            OnInGame();
        }

        if(gameState == GAMESTATE.ENDGAME)
        {
           OnEndGame();
        }
    }

    private void UpdateTime()
    {
        curTimeInterval += Time.deltaTime;
        if (curTimeInterval >= timeInterval)
        {
            TimeBattle += curTimeInterval;
            curTimeInterval = 0;
            ScreenBattle.Instance.UpdateTime();
        }
    }

    public void OnInGame()
    {
        switch (gameType)
        {
            case GAMETYPE.NORMAL:
                if (PlayerManagement.Instance.PlayerUnit.IsAlive() == false)
                {
                    SetGameState(GAMESTATE.ENDGAME);
                }
                else
                {
                    UpdateTime();
                }
                break;
            case GAMETYPE.COOP:
                if (CheckAPlayerIsDie())
                {
                    SetGameState(GAMESTATE.ENDGAME);
                }
                else
                {
                    UpdateTime();
                }
                break;
            default:
                Debug.Log("game type not found");
                break;
        }
    }
    
    public void OnStart()
    {
        TimeBattle = 0;
        curTimeInterval = 0;
        rewardGold = 3;
        gameType = GameClient.instance.gameType;

        if (GameClient.instance.StoreData.hasUpdateData && PlayerManagement.Instance.PlayerUnit != null)
        {
            SetGameState(GAMESTATE.INGAME);
        }

    }       

    public void OnLose()
    {
        ResultChapter resultBattle = new ResultChapter();
        resultBattle.chapterIdx = GameClient.instance.UInfo.GetCurrentChapter();
        resultBattle.result = ResultBattle.LOSE;
        resultBattle.perCompleted = 0;
        resultBattle.timeFinished = TimeBattle;
        resultBattle.goldCollected = rewardGold;
        
        GameClient.instance.RequestEndChapter(resultBattle);
    }
    
    public void OnWin()
    {
        ResultChapter resultBattle = new ResultChapter();
        resultBattle.chapterIdx = GameClient.instance.UInfo.GetCurrentChapter();
        resultBattle.result = ResultBattle.WIN;
        resultBattle.perCompleted = 0;
        resultBattle.timeFinished = TimeBattle;
        resultBattle.goldCollected = rewardGold;
        GameClient.instance.RequestEndChapter(resultBattle);
    }

    void OnEndGame()
    {
        switch (gameType)
        {
            case GAMETYPE.NORMAL:
                if (PlayerManagement.Instance.PlayerUnit.IsAlive() == false)
                {
                    OnLose();
                }
                else
                {
                    OnWin();
                }
                SetGameState(GAMESTATE.OUTBATTLE);
                break;
            case GAMETYPE.COOP:
                if (CheckAPlayerIsDie())
                {
                    OnLoseCoop();
                    SetGameState(GAMESTATE.OUTBATTLE);
                }
                else if(CheckWinBattle())
                {
                    OnWinCoop();
                    SetGameState(GAMESTATE.OUTBATTLE);
                }
                break;
            default:
                Debug.Log("game type not found");
                break;
        }
    }

    public void CollectGold(int quality = 1)
    {
        rewardGold += quality;
    }

    #region COOP  
    public bool CheckWinBattle()
    {
        var listPlayer = PhotonNetwork.PlayerList;
        for (int i = 0; i < listPlayer.Length; i++)
        {
            bool IsFinishBattle = true;
            if (listPlayer[i].CustomProperties.ContainsKey("FinishBattle"))
            {
                IsFinishBattle = (bool)listPlayer[i].CustomProperties["FinishBattle"];
                if (IsFinishBattle == false)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        return true;
    }
    public bool CheckAPlayerIsDie()
    {
        var listPlayer = PhotonNetwork.PlayerList;
        for (int i = 0; i < listPlayer.Length; i++)
        {
            bool IsLive = true;
            if (listPlayer[i].CustomProperties.ContainsKey("CurEnergyPlayer"))
            {
                Debug.Log(listPlayer[i].NickName + ": " + listPlayer[i].CustomProperties["CurEnergyPlayer"]);
                IsLive = (float)listPlayer[i].CustomProperties["CurEnergyPlayer"] > 0;
            }

            if (IsLive == false)
            {
                return true;
            }
        }
        return false;
    }

    public void OnLoseCoop()
    {
        Debug.Log("lose Coop");
        ResultChapter resultBattle = new ResultChapter();
        resultBattle.chapterIdx = GameClient.instance.UInfo.GetCurrentChapter();
        resultBattle.result = ResultBattle.LOSE;
        resultBattle.perCompleted = 0;
        resultBattle.timeFinished = TimeBattle;
        resultBattle.goldCollected = rewardGold;
        GameClient.instance.RequestEndPvP(resultBattle);
    }

    public void OnWinCoop()
    {
        Debug.Log("win Coop");
        ResultChapter resultBattle = new ResultChapter();
        resultBattle.chapterIdx = GameClient.instance.UInfo.GetCurrentChapter();
        resultBattle.result = ResultBattle.WIN;
        resultBattle.perCompleted = 0;
        resultBattle.timeFinished = TimeBattle;
        resultBattle.goldCollected = rewardGold;
        GameClient.instance.RequestEndPvP(resultBattle);
    }
    #endregion
}
