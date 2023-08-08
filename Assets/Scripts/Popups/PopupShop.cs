using System;
using System.Collections;
using System.Collections.Generic;
using Hara.GUI;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class PopupShop : PopupBase
{
    static PopupShop instance = null;
    [SerializeField] private List<NodePagination> nodePaginations;
    [SerializeField] private List<ListShopItem> listShop = new List<ListShopItem>();

    [Header("Close btn")] [SerializeField]
    private Button backBtn;

    private void Start()
    {
        backBtn.onClick.AddListener(() => OnCloseBtnClick(true));
    }

    public static PopupShop Create()
    {
        if (instance)
        {
            Destroy(instance.gameObject);
            instance = null;
        }

        var go = PopupManager.instance.GetPopup("PopupShop");
        instance = go.GetComponent<PopupShop>();
        instance.Init();
        return instance;
    }

    private void Init()
    {
        for (int i = 0; i < listShop.Count; i++)
        {
            listShop[i].SetGroupItem(nodePaginations[i]);
        }
    }
    
    
}
