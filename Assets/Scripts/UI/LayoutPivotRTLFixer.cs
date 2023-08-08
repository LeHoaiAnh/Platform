using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LayoutPivotRTLFixer : MonoBehaviour
{
    HorizontalOrVerticalLayoutGroup[] groups;
    AnchorPivotRTLSupport[] elements;
    TMP_Text[] texts;
    [SerializeField] GameObject[] rtlObjToogle;
    [SerializeField] Slider[] sliders;

    private void Awake()
    {
        groups = GetComponentsInChildren<HorizontalOrVerticalLayoutGroup>();
        elements = GetComponentsInChildren<AnchorPivotRTLSupport>(true);
        texts = GetComponentsInChildren<TMP_Text>(true);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (Localization.IsRTLLanguage())
        {
            foreach (var group in groups)
            {
                if (group == null) continue;
                if (group is HorizontalLayoutGroup)
                {
                    group.reverseArrangement = (group.reverseArrangement == false);
                }

                if (group.childAlignment == TextAnchor.UpperLeft)
                {
                    group.childAlignment = TextAnchor.UpperRight;
                }
                else if (group.childAlignment == TextAnchor.UpperRight)
                {
                    group.childAlignment = TextAnchor.UpperLeft;
                }
                else if (group.childAlignment == TextAnchor.MiddleLeft)
                {
                    group.childAlignment = TextAnchor.MiddleRight;
                }
                else if (group.childAlignment == TextAnchor.MiddleRight)
                {
                    group.childAlignment = TextAnchor.MiddleLeft;
                }
                else if (group.childAlignment == TextAnchor.LowerLeft)
                {
                    group.childAlignment = TextAnchor.LowerRight;
                }
                else if (group.childAlignment == TextAnchor.LowerRight)
                {
                    group.childAlignment = TextAnchor.LowerLeft;
                }
            }
            foreach (var t in elements)
            {
                if (t) t.ReversePivot();
            }

            foreach (var t in texts)
            {
                if (t.alignment == TextAlignmentOptions.Left)
                {
                    t.alignment = TextAlignmentOptions.Right;
                }
                else if (t.alignment == TextAlignmentOptions.Right)
                {
                    t.alignment = TextAlignmentOptions.Left;
                }
                else if (t.alignment == TextAlignmentOptions.TopLeft)
                {
                    t.alignment = TextAlignmentOptions.TopRight;
                }
                else if (t.alignment == TextAlignmentOptions.TopRight)
                {
                    t.alignment = TextAlignmentOptions.TopLeft;
                }
                else if (t.alignment == TextAlignmentOptions.BottomLeft)
                {
                    t.alignment = TextAlignmentOptions.BottomRight;
                }
                else if (t.alignment == TextAlignmentOptions.BottomRight)
                {
                    t.alignment = TextAlignmentOptions.BottomLeft;
                }

                var oldPivot = t.rectTransform.pivot;
                t.rectTransform.pivot = new Vector2(1f - oldPivot.x, oldPivot.y);
            }

            foreach (var t in rtlObjToogle)
            {
                if (t) t.SetActive(true);
            }

            foreach (var t in sliders)
            {
                if (t && t.direction == Slider.Direction.LeftToRight)
                {
                    t.direction = Slider.Direction.RightToLeft;
                }
                else
                if (t && t.direction == Slider.Direction.RightToLeft)
                {
                    t.direction = Slider.Direction.LeftToRight;
                }
                var tVal = t.value;
                t.value = 0f;
                t.value = tVal;
            }
        }
        else
        {
            //group.reverseArrangement = false;

            //foreach (var t in texts)
            //{
            //    //if (t.alignment == TextAlignmentOptions.Left)
            //    //{
            //    //    t.alignment = TextAlignmentOptions.Right;
            //    //}
            //    //else
            //    if (t.alignment == TextAlignmentOptions.Right)
            //    {
            //        t.alignment = TextAlignmentOptions.Left;
            //    }
            //}
        }
    }
}
