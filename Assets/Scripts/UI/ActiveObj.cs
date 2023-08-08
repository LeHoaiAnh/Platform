using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveObj : MonoBehaviour
{
    [SerializeField] private GameObject activeObj;
    [SerializeField] private GameObject deactiveObj;

    public void ShowActive()
    {
        activeObj.SetActive(true);
        deactiveObj.SetActive(false);
    }

    public void ShowDeactive()
    {
        activeObj.SetActive(false);
        deactiveObj.SetActive(true);
    }
}
