using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleTrigger : MonoBehaviour
{
    [SerializeField] private string message;

    private void OnCollisionEnter2D(Collision2D col)
    {
        PlayerUnit playerUnit = col.gameObject.GetComponent<PlayerUnit>();
        if (playerUnit != null)
        {
            PopupMessage.Create(MessagePopupType.INSTRUCTION, message);
        }
    }
}
