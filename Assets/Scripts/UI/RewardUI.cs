using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardUI : MonoBehaviour
{
    [SerializeField] private ShowItemCount item;

    public void HideAll()
    {
        item.gameObject.SetActive(false);
    }

    public void ShowItem(ItemReward reward)
    {
        HideAll();
        item.gameObject.SetActive(true);
        item.ShowItem(reward.type, reward.codename, reward.quality);
    }
       
}
