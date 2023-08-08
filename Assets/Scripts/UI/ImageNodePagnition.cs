using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageNodePagnition : MonoBehaviour
{
    [SerializeField] private Sprite label;
    [SerializeField] private Sprite labelActive;
    [SerializeField] private Image iconBG;
    [SerializeField] private Toggle m_Toggle;

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

  

    void ToggleValueChanged()
    {
        if (m_Toggle.isOn)
        {
            iconBG.sprite = labelActive;
        }
        else
        {
            iconBG.sprite = label;
        }
    }
}
