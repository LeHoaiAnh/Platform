using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hiker.GUI;
using Hiker.Networks.Data;
using System.Text;
using Hara.GUI;

public enum LoginType
{
    DEVICE,
    GOOGLE,
    APPLE,
    OFFLINE
}
public partial class GameClient : HTTPClient
{
    void DisplayerResponseMessage(Hiker.GUI.MessagePopupType msgType, string srvMessage)
    {
#if DEBUG
        Debug.Log(srvMessage);
#endif
        if (srvMessage.StartsWith("[LOCAL]"))
        {
            Hiker.GUI.PopupMessage.Create(Hiker.GUI.MessagePopupType.ERROR,
                Localization.Get(srvMessage.Substring(7)));
        }
        else
        {
            Hiker.GUI.PopupMessage.Create(Hiker.GUI.MessagePopupType.ERROR, srvMessage);
        }
    }

    bool ValidateResponse(ExDataBase response)
    {
        if (response == null)
        {
            Debug.Log("response is null");
            return false;
        }
        switch (response.ErrorCode)
        {
            case ERROR_CODE.OK:
                return true;
            case ERROR_CODE.INVALID_REQUEST:
                RequestGetUserInfo(UserInfo.ALL_PROPS, true);
                if (string.IsNullOrEmpty(response.ErrorMessage) == false)
                {
                    DisplayerResponseMessage(Hiker.GUI.MessagePopupType.TEXT, response.ErrorMessage);
                }
                return false;
            case ERROR_CODE.INVALIDATE_ACCESS_TOKEN:
                Debug.Log("Invalid Access Token");
                Hiker.GUI.PopupConfirm.Create(Localization.Get("INVALID_ACCESS_TOKEN"),
                    () =>
                    {
                        //LogOut();
                    },
                    false,
                    Localization.Get("BtnOk"));
                return false;
            case ERROR_CODE.GAME_VERSION_INVALID:
                ShowGameVersionUpdadePopup(response.ErrorMessage);
                return false;
            case ERROR_CODE.INVALID_INSTALL:
                ShowInvalidInstallPopup(response.ErrorMessage);
                return false;
            case ERROR_CODE.CONFIG_VERSION_INVALID:
                ShowConfigVersionUpdatePopup();
                return false;
            case ERROR_CODE.DISPLAY_MESSAGE:
                if (string.IsNullOrEmpty(response.ErrorMessage) == false)
                {
                    DisplayerResponseMessage(Hiker.GUI.MessagePopupType.TEXT, response.ErrorMessage);
                }
                return false;
            case ERROR_CODE.DISPLAY_MESSAGE_AND_QUIT:
                ShowPopupMessageAndQuit(response.ErrorMessage);
                return false;
            case ERROR_CODE.UNKNOW_ERROR:
                ProcessErrorResponse(response);
                return false;
            default:
                return true;
        }
    }

    void ProcessErrorResponse(ExDataBase response)
    {
        if (response == null)
        {
            DisplayerResponseMessage(Hiker.GUI.MessagePopupType.ERROR, "Response is null");
        }
        if (response.ErrorCode != ERROR_CODE.OK)
        {
            if (string.IsNullOrEmpty(response.ErrorMessage) == false)
            {
                DisplayerResponseMessage(Hiker.GUI.MessagePopupType.ERROR, response.ErrorMessage);
            }
            else
            {
                DisplayerResponseMessage(Hiker.GUI.MessagePopupType.ERROR,
                    Localization.Get(response.ErrorCode.ToString()));
            }
        }
    }



    public void RequestGetUserInfo(string[] props, bool isRunInBackground)
    {
        GetUserInfoRequest request = new GetUserInfoRequest();
        request.UpdateGIDData();
        request.Props = props;

        SendRequest("RequestGetUserInfo",
            LitJson.JsonMapper.ToJson(request),
            (data) =>
            {
                LoginResponse response = LitJson.JsonMapper.ToObject<LoginResponse>(data);
                if (ValidateResponse(response))
                {
                    if (response.ErrorCode == ERROR_CODE.OK)
                    {
                        Debug.Log("GetUserInfo");
                        //UpdateUserInfo(response.UInfo);
                        //if (GUIManager.Instance != null) GUIManager.Instance.UpdateTopGroup();
                    }
                    else
                    {
                        ProcessErrorResponse(response);
                    }
                }
            },
            isRunInBackground == false,
            isRunInBackground);
    }
  
    public void LoginDevice(string deviceID)
    {
        Login(LoginType.DEVICE, string.Empty, deviceID, null);
    }
    
    void Login(LoginType loginType, string userName, string deviceID, string[] datas)
    {
        LoginRequest request = new LoginRequest();
        request.UpdateGIDData();
        request.GID = 0;
        request.token = string.Empty;

        if (loginType == LoginType.APPLE)
        {
            request.AppleID = userName;
        }
        else if (loginType == LoginType.GOOGLE)
        {
            request.GoogleID = userName;
        }
        request.DeviceID = deviceID;
        request.Datas = datas;

        //LastLoginType = login_type;
        PlayerPrefs.SetInt("LoginType2", (int)loginType);
        PlayerPrefs.SetString("LoginDatas2", LitJson.JsonMapper.ToJson(request));
        bool accpetOffline = false;

        SendRequest("RequestLogin",
            LitJson.JsonMapper.ToJson(request),
            (data) =>
            {
                LoginResponse response = LitJson.JsonMapper.ToObject<LoginResponse>(data);
                if (ValidateResponse(response))
                {
                    if (response.ErrorCode == ERROR_CODE.OK)
                    {
                        Debug.Log("dang nhap thanh cong");
                        UInfo.GID = response.UInfo.GID;
                        UpdateUserInfo(response.UInfo);
                        GID = UInfo.GID;
                        AccessToken = response.Token;
                        if (PopupLogin.Instance != null && PopupLogin.Instance.gameObject.activeInHierarchy)
                        {
                            PopupLogin.Instance.OnCloseBtnClick();
                        }
                        GUIManager.Instance.SetScreen("Main");
                    }
                    else
                    {
                        ProcessErrorResponse(response);
                    }
                }
                else
                {
                    Debug.Log(data.ToString());
                }
            },
            showLoading: true,
            ignoreError: accpetOffline,
            (err, isNetworkError) =>
            {
                //LoginOffline();
                //_requestQueue.Enqueue("RequestLogin", LitJson.JsonMapper.ToJson(request));
            }, accept_offline: /*true*/ accpetOffline);
    }

    public void ShowGameVersionUpdadePopup(string url)
    {
        //client need upgrade
        PopupConfirm.Create("There is a new version of game. Please close the game and try to update", () =>
        {
            Application.Quit();
        }, false, "", "Need Update game");
    }

    public void ShowConfigVersionUpdatePopup()
    {
        //client need upgrade
        PopupConfirm.Create("There is a new version of game. Please close the game and try to update", () =>
        {
            Application.Quit();
        }, false, "", "Need Update game");
    }

    public void ShowPopupMessageAndQuit(string msg)
    {
        //client need upgrade
        PopupConfirm.Create(msg, () =>
        {
            Application.Quit();
        });
    }

    public void ShowInvalidInstallPopup(string url)
    {
        //client need upgrade
        PopupConfirm.Create("There is a new version of game. Please close the game and try to update", () =>
        {
            Debug.Log(url);
            Application.Quit();
        }, false);
    }
    
    public void UpdateUserInfo(UserInfo other)
    {
        LastTimeUpdateInfo = TimeUtils.Now;
        ServerTimeDiffTick = other.ServerTimeTick - LastTimeUpdateInfo.Ticks;
        UInfo.UpdateData(other);
        if (GUIManager.Instance != null)
        {
            GUIManager.Instance.UpdateTopGroup();
        }
    }
    
}
