using System.Collections;
using System.Collections.Generic;
using Hara.GUI;
using UnityEngine;

public class PopupLoading : PopupBase
{
    public static PopupLoading instance;
    
    public static PopupLoading Create()
    {
        if (instance)
        {
            Destroy(instance.gameObject);
            instance = null;
        }

        var go = PopupManager.instance.GetPopup("PopupLoading");
        instance = go.GetComponent<PopupLoading>();
        return instance;
    }

    public void Close()
    {
        OnCloseBtnClick();
    }
}
