using System;
using System.Collections;
using System.Collections.Generic;
using Hara.GUI;
using Photon.Pun;
using UnityEngine;

public class Exit : MonoBehaviourPun
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<PlayerUnit>() != null)
        {
            other.GetComponent<PlayerUnit>().PlayerFinishBattle();
        }
    }
}