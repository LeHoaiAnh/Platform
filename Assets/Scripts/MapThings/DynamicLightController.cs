using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class DynamicLightController : MapLightController
{
    public float lightSwitchIntervalTime = 3f;
    private float lightIntensityDecreasePerSecond;
    private float curIntervalTime;
    private float originIntensity;
    
    // Start is called before the first frame update
    protected override void Start()
    {
        typeOfLight = (TypeLight) Random.Range(0, (int)TypeLight.NUMS_OF_TYPES);
        base.Start();
        originIntensity = QuanLyManChoi.Instance.lightIntensity;
        curLight.intensity = originIntensity;
        lightIntensityDecreasePerSecond = originIntensity * 0.95f / lightSwitchIntervalTime;
        curIntervalTime = 0;
    }

    protected void Update()
    {
        curIntervalTime += Time.deltaTime;
        if (curIntervalTime >= lightSwitchIntervalTime)
        {
            curIntervalTime = 0;
            SwitchColor();
        }
        else
        {
            curLight.intensity -= lightIntensityDecreasePerSecond * Time.deltaTime;
            curLight.intensity = Mathf.Max(0, curLight.intensity);
        }
    }

    public void SwitchColor()
    {
        typeOfLight = (TypeLight)(((int)typeOfLight + 1) % (int)TypeLight.NUMS_OF_TYPES);
        SetLightColor();
        curLight.intensity = originIntensity;
        if (player != null)
        {
            OnTriggerEnterHelper(player);
        }
    }
}
