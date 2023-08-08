using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerBattle : MonoBehaviourPunCallbacks
{
    [SerializeField] private Slider slideEnergy;
    [SerializeField] private TextMeshProUGUI slideEnergyText;

    [SerializeField] private TextMeshProUGUI txtNamePlayer;
    Photon.Realtime.Player playerPvP;
  
    public void SetInfo(Photon.Realtime.Player _player)
    {
        playerPvP = _player;
        txtNamePlayer.text = playerPvP.NickName;
    }

    private float total = 1;
    private float current;
    public void Update()
    {
        if (PhotonNetwork.InRoom)
        {
            if (playerPvP.CustomProperties.ContainsKey("MaxEnergyPlayer") && playerPvP.CustomProperties.ContainsKey("CurEnergyPlayer"))
            {
                total = (float)playerPvP.CustomProperties["MaxEnergyPlayer"];
                current = (float)playerPvP.CustomProperties["CurEnergyPlayer"];
            }

            slideEnergyText.text = String.Format("{0} /{1}", Mathf.FloorToInt(current), Mathf.FloorToInt(total));
            slideEnergy.value = Mathf.Clamp01(current / total);
        }
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        if (playerPvP == otherPlayer)
        {
            Destroy(gameObject);
        }
    }

    public override void OnLeftRoom()
    {
        Destroy(gameObject);
    }
}
