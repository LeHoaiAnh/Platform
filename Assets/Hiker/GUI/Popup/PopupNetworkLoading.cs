using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Hiker.GUI
{
    using Hara.GUI;
    using UnityEngine.UI;

    public class PopupNetworkLoading : PopupBase
    {
        public Image spLoading;
        public TMPro.TMP_Text lbLoading;
        private float _TimeSinceRequest = float.MaxValue;

        public TweenAlpha groupLoading;

        public float TimeSinceRequest
        {
            get { return _TimeSinceRequest; }
            set { _TimeSinceRequest = value; }
        }

        static public PopupNetworkLoading instance = null;

        public static void Dismiss()
        {
            if (instance != null)
            {
                instance.Hide();
                instance = null;
            }
        }

        public static void Create(string str, float time = 20f, float delayShow = 0.5f, float transitionTime = 0.5f)
        {
            if (instance == null)
            {
                GameObject obj = PopupManager.instance.GetPopup("PopupNetworkLoading", false, Vector3.zero);
                instance = obj.GetComponent<PopupNetworkLoading>();
            }

            instance.lbLoading.text = str;
            instance.TimeSinceRequest = time;
            instance.gameObject.SetActive(true);
            instance.groupLoading.delay = delayShow;
            instance.groupLoading.duration = transitionTime;
            instance.groupLoading.ResetToBeginning();
            instance.groupLoading.Play(true);
        }
    }
}