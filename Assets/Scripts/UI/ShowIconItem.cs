using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class IconImg
{
    public TypeItem typeItem;
    public Image img;
}
public class ShowIconItem : MonoBehaviour
{
    public IconImg[] allImg;
    
    public void ShowImgIcon(TypeItem type, Sprite name)
    {
        for (int i = 0; i < allImg.Length; i++)
        {
            allImg[i].img.gameObject.SetActive(false);
        }

        IconImg found = allImg.First(e => e.typeItem == type);
        if (found != null)
        {
            found.img.gameObject.SetActive(true);
            found.img.sprite = name;
            if (type == TypeItem.Weapon)
            {
                found.img.SetNativeSize();
            }
            else
            {
                found.img.fillCenter = true;
            }
        }
    }
}
