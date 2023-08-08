using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemPlayerInRoom : MonoBehaviourPunCallbacks
{
    public TMP_Text txtName;
    Photon.Realtime.Player player;

    public void SetUp(Photon.Realtime.Player _player)
    {
        player = _player;
        txtName.text = player.NickName;
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        if (player == otherPlayer)
        {
            Destroy(gameObject);
        }
    }

    public override void OnLeftRoom()
    {
        Destroy(gameObject);
    }
}
