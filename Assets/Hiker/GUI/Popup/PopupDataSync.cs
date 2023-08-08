using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Hiker.GUI
{
    using Hara.GUI;
    using UnityEngine.UI;

    public class PopupDataSync : PopupBase
    {
        public TMPro.TMP_Text lbLoading;
        private float _TimeSinceRequest = float.MaxValue;

        public float TimeSinceRequest
        {
            get { return _TimeSinceRequest; }
            set { _TimeSinceRequest = value; }
        }
        static public PopupDataSync instance = null;

        public static void Dismiss()
        {
            if (instance != null)
            {
                instance.Hide();
                instance = null;
            }
        }

        public static void Create(string str, float time = 20f)
        {
            if (instance == null)
            {
                GameObject obj = PopupManager.instance.GetPopup("PopupDataSync", false, Vector3.zero);
                instance = obj.GetComponent<PopupDataSync>();
            }

            instance.lbLoading.text = str;
            instance.TimeSinceRequest = time;
            instance.gameObject.SetActive(true);
        }
    }
}