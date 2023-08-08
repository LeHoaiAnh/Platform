using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Hiker.GUI
{
    using Hara.GUI;
    using UnityEngine.UI;

    public class PopupToS : PopupBase
    {
        public GameObject grpTos;
        public GameObject grpDetail;

        public static PopupToS instance;

        public static void Create()
        {
            if (instance == null)
            {
                GameObject gObj = PopupManager.instance.GetPopup("PopupToS");
                instance = gObj.GetComponent<PopupToS>();
            }
            instance.grpTos.SetActive(true);
            instance.grpDetail.SetActive(false);
        }
        public static int GetTOSPref()
        {
            return PlayerPrefs.GetInt("TOS", 0);
        }


        [GUIDelegate]
        public override void OnBackBtnClick()
        {
            if (grpDetail.gameObject.activeSelf)
            {
                grpTos.SetActive(true);
                grpDetail.SetActive(false);
            }
            else
            {
                base.OnBackBtnClick();
            }
        }

        [GUIDelegate]
        public void OnDetailClick()
        {
            grpTos.SetActive(false);
            grpDetail.SetActive(true);
        }

        [GUIDelegate]
        public void OnTOSClick()
        {
            Application.OpenURL(Localization.Get("TOS_url_tos"));
        }

        [GUIDelegate]
        public void OnPolicyClick()
        {
            Application.OpenURL(Localization.Get("TOS_url_policy"));
        }
    }
}