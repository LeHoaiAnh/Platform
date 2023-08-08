using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShowItemCount : MonoBehaviour
{
    [SerializeField] private ShowItem showItem;
    [SerializeField] private TextMeshProUGUI textCount;
    
    public void ShowItem(TypeItem type, string name, int quality)
    {
        showItem.ShowImgIcon(type, name);
        textCount.text = quality.ToString();
    }
}
