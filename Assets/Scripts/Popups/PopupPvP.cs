using System;
using System.Collections;
using System.Collections.Generic;
using Hara.GUI;
using Hiker.Networks.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupPvP : PopupBase
{
    public static PopupPvP instance;

    public LauncherPvP launcherPvP;

    public static PopupPvP Create()
    {
        if (instance)
        {
            Destroy(instance.gameObject);
            instance = null;
        }

        var go = PopupManager.instance.GetPopup("PopupPvP");
        instance = go.GetComponent<PopupPvP>();
        return instance;
    }
}
