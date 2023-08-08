using System;
using System.Collections;
using System.Collections.Generic;
using HoaiAnh;
using Hara.GUI;
using Hiker.Networks.Data;
using Unity.VisualScripting;
using UnityEngine;
using Photon.Pun;

public class PlayerUnit : MonoBehaviourPun, IPunObservable
{
    public PlayerStat originStat;
    public PlayerStat curStat;
    bool mIsInited = false;
    ManagementEffect mStatusEffect;
    public PlayerLightController LightController { get; private set; }
    public EffectVisual[] EffectVisuals;
    public Vector3 OffsetHUD;
    public PlayerAppearance playerAppearance;
    #region Anim
    private Animator animator;
    private int mAnimInjuredBack;


    private bool Photnet_AnimSyncInjuredBack = false;
    public PhotonView PV;
    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        mAnimInjuredBack = Animator.StringToHash("InjuredBack");
    }
    #endregion
    private void OnEnable()
    {
        if (mIsInited == false)
            Init();
    }
    public ManagementEffect GetStatusEff()
    {
        if (mStatusEffect == null)
        {
            mStatusEffect = gameObject.AddMissingComponent<ManagementEffect>();
        }
        return mStatusEffect;
    }
   
    private void Init()
    {
        if (!GameClient.instance.StoreData.hasUpdateData)
        {
            return;
        }
        originStat = GameClient.instance.StoreData.startPlayerUnit;
        curStat = PlayerStat.Clone(originStat);

        if (LightController == null)
        {
            LightController = GetComponent<PlayerLightController>();
        }

        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }

        mIsInited = true;
        if (PhotonNetwork.InRoom)
        {
            if (PV.IsMine)
            {
                for (int i = 0; i < GameClient.instance.UInfo.itemEquips.Count; i++)
                {
                    ItemInventoryInfor item = GameClient.instance.UInfo.itemEquips[i];
                    PV.RPC("RPC_InitSkin", RpcTarget.AllBufferedViaServer, item.typeItem, item.codename);
                }

                if (myCustomProperties.ContainsKey("FinishBattle"))
                {
                    myCustomProperties["FinishBattle"] = false;
                }
                else
                {
                    myCustomProperties.Add("FinishBattle", false);
                }
                PhotonNetwork.LocalPlayer.SetCustomProperties(myCustomProperties);
            }
        }
        else
        {
            playerAppearance.Init();
        }
    }
    
    [PunRPC]
    public void RPC_InitSkin(TypeItem typeItem, string codeName)
    {
        playerAppearance.Init(typeItem, codeName);
    }

    public void UpdateConsumeSpeed(TypeLight typeLight)
    {
        if (typeLight == TypeLight.HEALING_LIGHT)
        {
            curStat.consumeSpeed = -ConfigManager.GetLightEnergySpeed();
        }
        else if (typeLight == TypeLight.NEUTRAL_LIGHT)
        {
            curStat.consumeSpeed = 0;
        }
        else if (typeLight == TypeLight.DAMAGING_LIGHT)
        {
            curStat.consumeSpeed = ConfigManager.GetLightEnergySpeed();
        }
    }
   
    public void ResetConsumeSpeed()
    {
        curStat.consumeSpeed = originStat.consumeSpeed;
    }

    public bool IsAlive()
    {
        return curStat.Energy > 0;
    }

    private void UpdateEnergy()
    {
        curStat.Energy -= curStat.consumeSpeed * Time.deltaTime;
        curStat.Energy = Mathf.Min(curStat.Energy, originStat.Energy);
        curStat.Energy = Mathf.Max(curStat.Energy, 0);
        ScreenBattle.Instance.UpdateEnergyPlayer();
    }
   
    public void GetEnergy(float amount)
    {        
        if(PhotonNetwork.InRoom && PV != null)
        {
            PV.RPC("PRC_GetEnergy", RpcTarget.AllBufferedViaServer, amount);
        }
        else
        {
            PRC_GetEnergy(amount);
        }
        
    }

    [PunRPC]
    public void PRC_GetEnergy(float amount)
    {
        if (amount < 0)
        {
            if (GetStatusEff().IsHaveActiveEffect(EffectType.Shield))
            {
                return;
            }
            GUIManager.Instance.PlaySound("Injured");
            ShowEffectDmg((long)amount, false);
        }
        curStat.Energy += amount;
        curStat.Energy = Mathf.Min(curStat.Energy, originStat.Energy);
        curStat.Energy = Mathf.Max(curStat.Energy, 0);
        if(PV != null && PV.IsMine == false)
        ScreenBattle.Instance.UpdateEnergyPlayer();
    }

    private void ShowEffectDmg(long dmg, bool isCrit)
    {
        animator.SetTrigger(mAnimInjuredBack);
        Photnet_AnimSyncInjuredBack = true;
        if (ScreenBattle.Instance)
            ScreenBattle.Instance.DisplayDmgHud(dmg, isCrit, this);
    }

    public void UpdateSpeed(float per)
    {
        curStat.movementVelocity +=  per / 100f * originStat.movementVelocity;
    }

    public void ResetSpeed()
    {
        curStat.movementVelocity = originStat.movementVelocity;
    }
    private void Update()
    {
        if (PhotonNetwork.InRoom)
        {
            if (PV != null && PV.IsMine == false) return;
        }

        if (PlayerManagement.Instance.loading) return;
        if (!mIsInited)
        {
            Init();
        }
        if (IsAlive())
        {
            UpdateEnergy();
            ProcessHud();
        }
        SetPropertiesPvP();
        if (IsAlive() == false)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }
   
    private Queue<ScreenBattle.DisplayHUDCommand> queueHUDs = new Queue<ScreenBattle.DisplayHUDCommand>();

    float hudCoolDown = 0;
    private void ProcessHud()
    {
        if (hudCoolDown > 0)
        {
            hudCoolDown -= Time.unscaledDeltaTime;
        }
        else
        {
            if (queueHUDs.Count > 0)
            {
                var command = queueHUDs.Dequeue();
                var originText = command.Text;
                ScreenBattle.Instance.DisplayHUD(command);
                hudCoolDown = 0.065f;
            }
        }
    }
    
    public void QueueHudDisplay(ScreenBattle.DisplayHUDCommand hudCommand)
    {
        queueHUDs.Enqueue(hudCommand);
    }

    public int GetCurrentHUDQueue()
    {
        return queueHUDs.Count;
    }

    public long TakeDamage(SatThuongInfo st)
    {
        var takenDmg = 0L;
        if (IsAlive() == false) return takenDmg;

        var dmg = st.dmg;
        GetEnergy(-dmg);
        return dmg;
    }

    private void OnDisable()
    {
        mIsInited = false;
    }

    public ExitGames.Client.Photon.Hashtable myCustomProperties = new ExitGames.Client.Photon.Hashtable();
    private void SetPropertiesPvP()
    {
        if (PhotonNetwork.InRoom)
        {
            if (PV != null && PV.IsMine == false) return;
        }
        if (myCustomProperties.ContainsKey("CurEnergyPlayer"))
        {
            myCustomProperties["CurEnergyPlayer"] = curStat.Energy;
        }
        else
        {
            myCustomProperties.Add("CurEnergyPlayer", curStat.Energy);
        }

        if (myCustomProperties.ContainsKey("MaxEnergyPlayer"))
        {
            myCustomProperties["MaxEnergyPlayer"] = originStat.Energy;
        }
        else
        {
            myCustomProperties.Add("MaxEnergyPlayer", originStat.Energy);
        }
        PhotonNetwork.LocalPlayer.SetCustomProperties(myCustomProperties);
    }

    public void PlayerFinishBattle()
    {
        if (PhotonNetwork.InRoom)
        {
            if (PV != null && PV.IsMine == false)
            {
                return;
            }
            if (myCustomProperties.ContainsKey("FinishBattle"))
            {
                myCustomProperties["FinishBattle"] = true;
            }
            else
            {
                myCustomProperties.Add("FinishBattle", true);
            }
            PhotonNetwork.LocalPlayer.SetCustomProperties(myCustomProperties);
            PhotonNetwork.Destroy(gameObject);
        }
        QuanLyManChoi.Instance.SetGameState(GAMESTATE.ENDGAME);
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(mIsInited);

            stream.SendNext(curStat.Energy);
            stream.SendNext(originStat.Energy);

            stream.SendNext(Photnet_AnimSyncInjuredBack);
            Photnet_AnimSyncInjuredBack = false;
        }
        else if (stream.IsReading)
        {
            mIsInited = (bool)stream.ReceiveNext();

            curStat.Energy = (float)stream.ReceiveNext();
            originStat.Energy = (float)stream.ReceiveNext();

            Photnet_AnimSyncInjuredBack = (bool)stream.ReceiveNext();
            if (Photnet_AnimSyncInjuredBack)
            {
                animator.SetTrigger(mAnimInjuredBack);
                Photnet_AnimSyncInjuredBack = false;
            }
        }
    }
}
