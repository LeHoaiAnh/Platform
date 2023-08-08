using System;
using System.Collections;
using System.Collections.Generic;
using Hiker.Networks.Data;
using Unity.VisualScripting;
using UnityEngine;

public class StoreData : MonoBehaviour
{
    public bool hasUpdateData;
    public PlayerStat startPlayerUnit;

    private void OnEnable()
    {
        ResetData();
    }

    public void UpdateData(PlayerStat data)
    {
        startPlayerUnit = data;
        hasUpdateData = true;
    }

    public void ResetData()
    {
        hasUpdateData = false;
        startPlayerUnit = null;
    }
}
