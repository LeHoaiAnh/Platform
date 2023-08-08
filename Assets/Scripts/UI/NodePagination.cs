using Hiker.GUI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NodePagination : MonoBehaviour
{
    [SerializeField] private Sprite label;
    [SerializeField] private Sprite labelActive;

    [SerializeField] private Image iconBG;
    [SerializeField] private TMP_Text textDes;
    [SerializeField] private Toggle m_Toggle;
    [SerializeField] private Color textColor;
    [SerializeField] private Color textColorActive;
    private int _index;

    // Start is called before the first frame update
    void Start()
    {
        m_Toggle.onValueChanged.AddListener(delegate
        {
            ToggleValueChanged();
        });

        ToggleValueChanged();
    }

    public void SetItem(TypeItem typeItem)
    {
        textDes.text = typeItem.ToString();
    }


    void ToggleValueChanged()
    {
        if (m_Toggle.isOn)
        {
            iconBG.sprite = labelActive;
            textDes.color = textColorActive;
        }
        else
        {
            iconBG.sprite = label;
            textDes.color = textColor;
        }
    }
}
