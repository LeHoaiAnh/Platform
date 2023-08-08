using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupChapter : MonoBehaviour
{
    [SerializeField] private Transform container;
    [SerializeField] private SelectChapterItem itemPref;
    
    public void SetGroupChapter( int start, int end)
    {
        container.DestroyChildren();

        for (int i = start; i < end; i++)
        {
            var obj = SimplePool.Spawn(itemPref.gameObject, container);
            var objItem = obj.GetComponent<SelectChapterItem>();
            objItem.SetChapter(i);
        }
    }
}
