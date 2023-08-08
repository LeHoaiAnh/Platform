using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ScreenBattlePvP : MonoBehaviourPunCallbacks, IPunObservable
{
    public static ScreenBattlePvP instance;

    public List<UIPlayerBattle> listUIPlayerBattles = new List<UIPlayerBattle>();
    public int numBtn = 0;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("error");
            return;
        }
        instance = this;
    }

    public Transform playerListContent;
    [SerializeField] private UIPlayerBattle playerBattlePrefab;
    public override void OnJoinedRoom()
    {
        Photon.Realtime.Player[] players = PhotonNetwork.PlayerList;
        numBtn = 0;
        for (int i = 0; i < players.Length; i++)
        {
            UIPlayerBattle uIPlayerBattle = null;
            if (listUIPlayerBattles.Count > i)
            {
                uIPlayerBattle = listUIPlayerBattles[i];
            }
            if (uIPlayerBattle == null)
            {
                uIPlayerBattle = Instantiate(playerBattlePrefab, playerListContent);
                if (listUIPlayerBattles.Count <= i)
                {
                    listUIPlayerBattles.Add(uIPlayerBattle);
                }
                else
                {
                    listUIPlayerBattles[i] = uIPlayerBattle;
                }
            }

            numBtn++;
            if (uIPlayerBattle)
            {
                uIPlayerBattle.SetInfo(players[i]);
                uIPlayerBattle.gameObject.SetActive(true);
            }
        }

        for (int i = numBtn; i < listUIPlayerBattles.Count; ++i)
        {
            if (listUIPlayerBattles[i]) listUIPlayerBattles[i].gameObject.SetActive(false);
        }
    }

    public override void OnEnable()
    {
        base.OnEnable();
        OnJoinedRoom();
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        UIPlayerBattle uIPlayerBattle = null;
        if (listUIPlayerBattles.Count > numBtn)
        {
            uIPlayerBattle = listUIPlayerBattles[numBtn];
        }
        if (uIPlayerBattle == null)
        {
            uIPlayerBattle = Instantiate(playerBattlePrefab, playerListContent);
            if (listUIPlayerBattles.Count <= numBtn)
            {
                listUIPlayerBattles.Add(uIPlayerBattle);
            }
            else
            {
                listUIPlayerBattles[numBtn] = uIPlayerBattle;
            }
        }

        numBtn++;
        if (uIPlayerBattle)
        {
            uIPlayerBattle.SetInfo(newPlayer);
            uIPlayerBattle.gameObject.SetActive(true);
        }
        for (int i = numBtn; i < listUIPlayerBattles.Count; ++i)
        {
            if (listUIPlayerBattles[i]) listUIPlayerBattles[i].gameObject.SetActive(false);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

    }
}
