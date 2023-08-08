using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ShowTypeItem : MonoBehaviour
{
    [SerializeField] private Image bg;
    public IconImg[] allImg;

    public void ShowImgIcon(TypeItem type, ERarity rarity)
    {
        bg.sprite = GameClient.instance.spriteCollection.GetSprite("Frame_" + rarity);
        for (int i = 0; i < allImg.Length; i++)
        {
            allImg[i].img.gameObject.SetActive(false);
        }

        IconImg found = allImg.First(e => e.typeItem == type);
        if (found != null)
        {
            found.img.gameObject.SetActive(true);
        }
    }
}
