using System;
using System.Collections;
using System.Collections.Generic;
using Hara.GUI;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PopupNhanThuong : PopupBase
{
    static PopupNhanThuong instance = null;
    [SerializeField] private GameObject rewardPrefab;
    [SerializeField] private Transform container;
    [SerializeField] private Button continueBtn;

    private void Start()
    {
        continueBtn.onClick.AddListener(OnPressContinue);
    }

    private void OnPressContinue()
    {
        OnCloseBtnClick(true);
    }

    public static PopupNhanThuong Create(GeneralReward reward)
    {
        if (instance)
        {
            Destroy(instance.gameObject);
            instance = null;
        }

        var go = PopupManager.instance.GetPopup("PopupNhanThuong");
        instance = go.GetComponent<PopupNhanThuong>();
        instance.Init(reward);
        return instance;
    }

    private void Init(GeneralReward reward)
    {
        RewardUI[] rewardUis = container.GetComponentsInChildren<RewardUI>();
        for (int i = 0; i < rewardUis.Length; i++)
        {
            SimplePool.Despawn(rewardUis[i].gameObject);
        }
        if (reward.items.Length > 0)
        {
            for (int i = 0; i < reward.items.Length; i++)
            {
                var obj = SimplePool.Spawn(rewardPrefab, container);
                var objUI = obj.GetComponent<RewardUI>();
                objUI.ShowItem(reward.items[i]);
            }
        }
    }
}
