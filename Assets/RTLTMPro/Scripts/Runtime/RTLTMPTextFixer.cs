using RTLTMPro;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace RTLTMPro
{
    [RequireComponent(typeof(TMPro.TMP_Text))]
    [ExecuteInEditMode]
    public class RTLTMPTextFixer : MonoBehaviour
    {
        [SerializeField][TextArea(3, 10)] protected string originalText;

        [SerializeField] protected bool preserveNumbers = true;

        [SerializeField] protected bool farsi = true;

        [SerializeField] protected bool fixTags = true;

        [SerializeField] protected bool forceFix;

        static readonly FastStringBuilder finalText = new FastStringBuilder(RTLSupport.DefaultBufferSize);

        public string text
        {
            get { return originalText; }
            set { originalText = value; UpdateText(); }
        }

        public bool havePropertiesChanged { get; set; }

        public string OriginalText
        {
            get { return originalText; }
        }

        public bool PreserveNumbers
        {
            get { return preserveNumbers; }
            set
            {
                if (preserveNumbers == value)
                    return;

                preserveNumbers = value;
                havePropertiesChanged = true;
            }
        }

        public bool Farsi
        {
            get { return farsi; }
            set
            {
                if (farsi == value)
                    return;

                farsi = value;
                havePropertiesChanged = true;
            }
        }

        public bool FixTags
        {
            get { return fixTags; }
            set
            {
                if (fixTags == value)
                    return;

                fixTags = value;
                havePropertiesChanged = true;
            }
        }

        public bool ForceFix
        {
            get { return forceFix; }
            set
            {
                if (forceFix == value)
                    return;

                forceFix = value;
                havePropertiesChanged = true;
            }
        }

        TMP_Text tmpTxt;

        public string GetFixedText(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            finalText.Clear();
            RTLSupport.FixRTL(input, finalText, farsi, fixTags, preserveNumbers);
            finalText.Reverse();
            return finalText.ToString();
        }

        private void Awake()
        {
            if (tmpTxt == null)
            {
                tmpTxt = GetComponent<TMP_Text>();
            }
        }

        private void Start()
        {
            //// force language to Arabic for testing purpose
            //Localization.language = "Arabic";
            //FixText();
        }

        private void Update()
        {
            if (havePropertiesChanged)
            {
                UpdateText();
            }
        }

        public void UpdateText()
        {
            if (tmpTxt == null) return;

            if (originalText == null)
                originalText = string.Empty;

            if (forceFix == false && TextUtils.IsRTLInput(originalText) == false)
            {
                tmpTxt.isRightToLeftText = false;
                tmpTxt.text = originalText;
            }
            else
            {
                tmpTxt.isRightToLeftText = true;
                tmpTxt.text = GetFixedText(originalText);
            }
        }
    }
}