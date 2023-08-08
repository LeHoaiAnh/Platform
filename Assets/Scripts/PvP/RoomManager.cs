using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public static RoomManager Instance;
    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        Instance = this;
    }
   
    public override void OnEnable()
    {
        base.OnEnable();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (scene.buildIndex > 0) // We're in the game scene
        {
            if(PopupPvP.instance) PopupPvP.instance.OnCloseBtnClick();
        }
    }

    public override void OnLeftLobby()
    {
        Debug.Log("Left Lobby");
    }
    public override void OnDisconnected(DisconnectCause cause)
    {       
        base.OnDisconnected(cause);
        Debug.Log("Disconnected");
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(0);
        //PhotonNetwork.Disconnect();
    }

    private void Recover()
    {
        if (PhotonNetwork.NetworkingClient.ReconnectAndRejoin())
        {
            if (PhotonNetwork.NetworkingClient.ReconnectToMaster() == false)
            {

            }
        }
    }
}
