using System;
using System.Collections;
using System.Collections.Generic;
using Hara.GUI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupLogin : PopupBase
{ 
   [Header("Login")]
   [SerializeField] private TMP_InputField inputFieldName;
   [SerializeField] private Button btnLogin;

   public static PopupLogin Instance = null;

   public static void Create()
   {
       if (Instance == null)
       {
           GameObject go = PopupManager.instance.GetPopup("PopupLogin");
           Instance = go.GetComponent<PopupLogin>();
       }
   }
   private void Start()
   {
      btnLogin.onClick.AddListener(OnBtnLoginClick);
   }
   
   public void OnBtnLoginClick()
   {
       var curVal = inputFieldName.text;
       curVal = curVal.Trim();
       if (string.IsNullOrEmpty(curVal) == false)
       {
           GameClient.instance.LoginDevice(curVal);
       }
    }

}
