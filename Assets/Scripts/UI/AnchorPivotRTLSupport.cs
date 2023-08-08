using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class AnchorPivotRTLSupport : MonoBehaviour
{
    RectTransform rect;
    UIFlippable flippable;

    private void OnEnable()
    {
        if (rect == null)
        {
            rect = GetComponent<RectTransform>();
        }
        if (flippable == null)
            flippable = GetComponent<UIFlippable>();
    }

    static bool Approximately(Vector2 a, Vector2 b)
    {
        return Mathf.Approximately(a.x, b.x) && Mathf.Approximately(a.y, b.y);
    }

    static bool IsAnchorAtPoint(RectTransform trans)
    {
        return Approximately(trans.anchorMin, trans.anchorMax);
    }

    public void ReversePivot()
    {
        if (rect == null) rect = GetComponent<RectTransform>();
        if (IsAnchorAtPoint(rect))
        {
            var anchorPoint = rect.anchoredPosition;
            rect.anchorMin = rect.anchorMax = new Vector2(1f - rect.anchorMax.x, rect.anchorMax.y);
            rect.anchoredPosition = new Vector2(-anchorPoint.x, anchorPoint.y);
        }

        if (flippable == null) flippable = GetComponent<UIFlippable>();
        if (flippable)
        {
            flippable.horizontal = !flippable.horizontal;
        }
    }
}
