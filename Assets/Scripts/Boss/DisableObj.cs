using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableObj : MonoBehaviour
{
    [SerializeField] private GameObject obstacle;
    private BossController bossController;

    private void Awake()
    {
        bossController = GetComponent<BossController>();
    }

    private void OnEnable()
    {
        bossController.OnUnitDieAction += DisableObstacle;
    }

    void DisableObstacle()
    {
        obstacle.SetActive(false);
    }
    private void OnDisable()
    {
        bossController.OnUnitDieAction -= DisableObstacle;
    }
}
