using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TargetST
{
    Player, 
    Enemy
}

public enum LoaiST
{
    Melee,
    Bullet
}
public class GayDamKhiVaCham : MonoBehaviour
{
    [SerializeField] private TargetST targetSt;
    [SerializeField] private LoaiST loaiSt;
    private PlayerUnit playerUnit;
    private BossController bossUnit;
    private void Start()
    {
        if (targetSt == TargetST.Enemy)
        {
            playerUnit = GetComponentInParent<PlayerUnit>();
            bossUnit = null;
        }
        else
        {
            bossUnit = GetComponentInParent<BossController>();
            playerUnit = null;
        }
    }

    private long GetDMG()
    {
        long dmg = 0;
        if (targetSt == TargetST.Enemy)
        {
            if (loaiSt == LoaiST.Melee)
            {
                dmg = playerUnit.curStat.meleeDmg;
            } 
        }

        if (targetSt == TargetST.Player)
        {
            dmg = bossUnit.currentStats.attackDmg;
        }

        return dmg;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (PhotonNetwork.InRoom && PhotonNetwork.IsMasterClient == false)
        {
            return;
        }

        if (targetSt == TargetST.Enemy)
        {
            BossController bossController = collider.GetComponent<BossController>();
            if(bossController != null)
            {
                Debug.Log(GetDMG());
                bossController.UpdateHP(-GetDMG());
            }
        }

        if (targetSt == TargetST.Player)
        {
            PlayerUnit player = collider.GetComponent<PlayerUnit>();
            if (player != null)
            {
                player.GetEnergy(-GetDMG());
            }
        }
    }
}
