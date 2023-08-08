using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnFXOnEnable : MonoBehaviour
{
    [SerializeField] GameObject fxPrefab;
    [SerializeField] Vector3 fxPos;

    GameObject fxObj = null;
    public GameObject GetFXPrefab()
    {
        return fxPrefab;
    }

    private void OnEnable()
    {
        if (fxObj == null)
        {
            fxObj = SimplePool.Spawn(fxPrefab, transform, true);
            fxObj.transform.localPosition = fxPos;
            fxObj.transform.localScale = Vector3.one;
            fxObj.transform.localRotation = Quaternion.identity;
        }
        else
        {
            fxObj.gameObject.SetActive(true);
        }
    }

    private void OnDisable()
    {
        if (fxObj)
        {
            fxObj.gameObject.SetActive(false);
            SimplePool.Despawn(fxObj);
            fxObj = null;
        }
    }
}
