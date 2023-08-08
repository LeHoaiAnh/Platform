using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class LauncherPvP : MonoBehaviourPunCallbacks
{
    [Header("Loading")]
    public GameObject loading;

    [Header("Lobby")]
    public GameObject lobby;

    [Header("Room")]
    public GameObject room;
    public ItemPlayerInRoom itemPlayerInRoomPrefab;
    public Transform containerListPlayerInRoom;

    protected int maxPlayer = 2;
    //private LoadBalancingClient loadBalancingClient;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("is start");
        loading.SetActive(true);
        lobby.SetActive(false);
        room.SetActive(false);
        //PhotonNetwork.AutomaticallySyncScene = true;
        //loadBalancingClient = PhotonNetwork.NetworkingClient;
        if (PhotonNetwork.IsConnected == false)
        {
            PhotonNetwork.SendRate = 50;
            PhotonNetwork.SerializationRate = 50;
            PhotonNetwork.ConnectUsingSettings();
        }

        if(PhotonNetwork.InLobby == false)
        {
            OnConnectedToMaster();
        }
        else
        {
            OnJoinedLobby();
        }

        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
        }
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master");
        PhotonNetwork.JoinLobby();
        PhotonNetwork.NickName = GameClient.instance.UInfo.gamer.Name;
    }

    public override void OnJoinedLobby()
    {
        lobby.SetActive(true);
        loading.SetActive(false);
        Debug.Log("Joined Lobby");
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("Room List Update: " + roomList.Count);
    }
    public override void OnLeftLobby()
    {
        Debug.Log("Left Lobby: PopupPvP");
        PhotonNetwork.Disconnect();
    }
    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        room.SetActive(false);
    }
    public override void OnJoinedRoom()
    {        
        room.SetActive(true);
        Debug.Log("Joined Room");

        containerListPlayerInRoom.DestroyChildren();
        var players = PhotonNetwork.PlayerList;
        for (int i = 0; i < players.Length; i++)
        {
            var clone = Instantiate(itemPlayerInRoomPrefab, containerListPlayerInRoom);
            clone.gameObject.SetActive(true);
            clone.SetUp(players[i]);
        }

        StartGamePvP();
    }

    public override void OnMasterClientSwitched(Photon.Realtime.Player newMasterClient)
    {
        Debug.Log("Switched Master In Room");
    }
    public void CreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;
        EnterRoomParams enterRoomParams = new EnterRoomParams();
        enterRoomParams.RoomOptions = roomOptions;
        PhotonNetwork.NetworkingClient.OpCreateRoom(enterRoomParams);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Room Creation Failed: " + message);
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        Debug.Log("new Player Entered Room: " + newPlayer.NickName);
        var clone = Instantiate(itemPlayerInRoomPrefab, containerListPlayerInRoom);
        clone.gameObject.SetActive(true);
        clone.SetUp(newPlayer);

        StartGamePvP();
    }

    public void FindMatch()
    {
        Debug.Log("find match");
        PhotonNetwork.NetworkingClient.OpJoinRandomOrCreateRoom(null, null);

    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("Random join failed");
    }

    public void StartGamePvP()
    {
        if (PhotonNetwork.PlayerList.Length >= maxPlayer)
        {
            GameClient.instance.UInfo.SetCurrentChapter(0);
            GameClient.instance.RequestStartPvP(0);
        }

    }
}
