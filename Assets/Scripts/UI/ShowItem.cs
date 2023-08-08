using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowItem : MonoBehaviour
{
    [SerializeField] private Image bg;
    [SerializeField] private ShowIconItem showIconItem;
    public ItemStatsInfor statsInfor { get; set; }
    public void ShowImgIcon(TypeItem type, string name)
    {
        statsInfor = ConfigManager.itemStatsCfg.GetItemCfg(type, name);
        showIconItem.ShowImgIcon(type, GameClient.instance.spriteCollection.GetSprite(type + "_" + name));
        bg.sprite = GameClient.instance.spriteCollection.GetSprite("Frame_" + statsInfor.Rarity);
    }

}
