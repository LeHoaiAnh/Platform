using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TopGroup : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI goldText;
    private long goldOnText;

    public void UpdateTopGroup()
    {
        UpdateCurrentGold();
    }
    
    public void UpdateCurrentGold()
    {
        goldOnText = GameClient.instance.UInfo.GetCurrentGold();
        goldText.text = ConfigManager.RoundNumberToInt(goldOnText);
    }
}
