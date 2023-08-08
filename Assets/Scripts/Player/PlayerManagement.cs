using System;
using System.Collections;
using System.Collections.Generic;
using HoaiAnh;
using Hara.GUI;
using UnityEngine;
using UnityEngine.Serialization;
using Photon.Pun;

public class PlayerManagement : MonoBehaviour
{
    public static PlayerManagement Instance {
        get {
            if (instance == null)
                instance = FindObjectOfType<PlayerManagement>();
            return instance;
        }
    }
    static PlayerManagement instance = null;
    
    [Header("Set up Camera")]
    public TargetFollower CamFollower;
    public Transform CamOriginPos;

    public bool loading { get; set; }
    [Header("Set up Player")] public PlayerUnit PlayerUnit;
    public Transform startPos;

    private void Awake()
    {
        
    }
    private void OnEnable()
    {
        loading = true;
        GUIManager.Instance.SetScreen("Battle");
        ObjectPoolManager.Init();
        LoadPlayerUnit();
        Utils.DoAction(this, () => { loading = false; }, 1f, true);
    }
    
    private void LoadPlayerUnit()
    {
        GameObject clonePlayer = null;
        if (PhotonNetwork.InRoom)
        {
            clonePlayer = PhotonNetwork.Instantiate("Prefabs/Player", startPos.position, Quaternion.identity);
        }
        else
        {
            clonePlayer = Instantiate(Resources.Load<GameObject>("Prefabs/Player"), startPos.position, Quaternion.identity);
        }

        PlayerUnit = clonePlayer.GetComponent<PlayerUnit>();
        PlayerUnit.gameObject.SetActive(true);

        CamFollower.transform.position = CamOriginPos.position;
        CamFollower.target = PlayerUnit.transform;
        CamFollower.InitPos();
    }
}

public static class LayersMan
{
    public static int LayerWall = LayerMask.GetMask("Wall");
    public static int LayerGround = LayerMask.GetMask("Ground");
}