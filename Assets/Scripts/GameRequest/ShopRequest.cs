using System.Collections;
using System.Collections.Generic;
using Hiker.Networks.Data;
using Photon.Pun;
using UnityEngine;

public partial class GameClient : HTTPClient
{
    public void RequestBuyInventory(TypeItem typeItem, string codename, int quality)
    {
        BuyRequest request = new BuyRequest();
        request.UpdateGIDData();
        request.typeItem = typeItem;
        request.codename = codename;
        request.quality = quality;
        
        SendRequest("RequestBuyInventory",
            LitJson.JsonMapper.ToJson(request),
            (data) =>
            {
                BuyResponse response = LitJson.JsonMapper.ToObject<BuyResponse>(data);
                if (ValidateResponse(response))
                {
                    if (response.ErrorCode == ERROR_CODE.OK)
                    {
                        UpdateUserInfo(response.UInfo);
                        PopupNhanThuong.Create(response.reward);
                    }
                    else
                    {
                        ProcessErrorResponse(response);
                    }
                }
            },true,
            false);
    }
    
    public void RequestEquipItem(TypeItem typeItem, string codename, bool equip)
    {
        EquipItemRequest request = new EquipItemRequest();
        request.UpdateGIDData();
        request.type = typeItem;
        request.codename = codename;
        request.equip = equip;
        
        SendRequest("RequestEquipItem",
            LitJson.JsonMapper.ToJson(request),
            (data) =>
            {
                UserInfoResponse response = LitJson.JsonMapper.ToObject<UserInfoResponse>(data);
                if (ValidateResponse(response))
                {
                    if (response.ErrorCode == ERROR_CODE.OK)
                    {
                        UpdateUserInfo(response.UInfo);
                        if (PopupInventory.Instance != null && PopupInventory.Instance.gameObject.activeInHierarchy)
                        {
                            PopupInventory.Instance.UpdateVisual();
                        }
                    }
                    else
                    {
                        ProcessErrorResponse(response);
                    }
                }
            },true,
            false);
    }

    public void RequestForge(List<ForgeMaterial> materials)
    {
        ForgeRequest request = new ForgeRequest();
        request.UpdateGIDData();
        request.materials = materials;
        
        SendRequest("RequestForge",
            LitJson.JsonMapper.ToJson(request),
            (data) =>
            {
                ForgeResponse response = LitJson.JsonMapper.ToObject<ForgeResponse>(data);
                if (ValidateResponse(response))
                {
                    if (response.ErrorCode == ERROR_CODE.OK)
                    {
                        UpdateUserInfo(response.UInfo);
                        PopupNhanThuong.Create(response.reward);
                        if (PopupForge.instance != null && PopupForge.instance.gameObject.activeInHierarchy)
                        {
                            PopupForge.instance.Init();
                        }
                    }
                    else
                    {
                        ProcessErrorResponse(response);
                    }
                }
            },true,
            false);
    }
    
    public void RequestStartChapter(int chapter)
    {
        ChapterStartRequest request = new ChapterStartRequest();
        request.UpdateGIDData();
        request.chapter = chapter;
        
        SendRequest("RequestStartChapter",
            LitJson.JsonMapper.ToJson(request),
            (data) =>
            {
                ChapterStartResponse response = LitJson.JsonMapper.ToObject<ChapterStartResponse>(data);
                if (ValidateResponse(response))
                {
                    if (response.ErrorCode == ERROR_CODE.OK)
                    {
                        UpdateUserInfo(response.UInfo);
                        StoreData.UpdateData(response.PlayerUnit);
                        gameType = GAMETYPE.NORMAL;
                    }
                    else
                    {
                        ProcessErrorResponse(response);
                    }
                }
            },true,
            false);
    }
    
    public void RequestEndChapter(ResultChapter resultChapter)
    {
        ChapterEndRequest request = new ChapterEndRequest();
        request.UpdateGIDData();
        request.resultChapter = resultChapter;
        
        SendRequest("RequestEndChapter",
            LitJson.JsonMapper.ToJson(request),
            (data) =>
            {
                ChapterEndResponse response = LitJson.JsonMapper.ToObject<ChapterEndResponse>(data);
                if (ValidateResponse(response))
                {
                    if (response.ErrorCode == ERROR_CODE.OK)
                    {
                        StoreData.ResetData();
                        UpdateUserInfo(response.UInfo);
                        PopupEndGame.Create(request.resultChapter, response.star);
                    }
                    else
                    {
                        ProcessErrorResponse(response);
                    }
                }
            },true,
            false);
    }

    public void RequestStartPvP(int chapter)
    {
        ChapterStartRequest request = new ChapterStartRequest();
        request.UpdateGIDData();
        request.chapter = chapter;

        SendRequest("RequestStartPvP",
            LitJson.JsonMapper.ToJson(request),
            (data) =>
            {
                ChapterStartResponse response = LitJson.JsonMapper.ToObject<ChapterStartResponse>(data);
                if (ValidateResponse(response))
                {
                    if (response.ErrorCode == ERROR_CODE.OK)
                    {
                        UpdateUserInfo(response.UInfo);
                        StoreData.UpdateData(response.PlayerUnit);
                        gameType = GAMETYPE.COOP;
                        //if (Photon.Pun.PhotonNetwork.IsMasterClient)
                        //{
                            Photon.Pun.PhotonNetwork.LoadLevel(4);
                        //}
                        //UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(4);
                    }
                    else
                    {
                        ProcessErrorResponse(response);
                    }
                }
            }, true,
            false);
    }

    public void RequestEndPvP(ResultChapter resultChapter)
    {
        //ChapterEndRequest request = new ChapterEndRequest();
        //request.UpdateGIDData();
        //request.resultChapter = resultChapter;

        //SendRequest("RequestEndChapter",
        //    LitJson.JsonMapper.ToJson(request),
        //    (data) =>
        //    {
        //        ChapterEndResponse response = LitJson.JsonMapper.ToObject<ChapterEndResponse>(data);
        //        if (ValidateResponse(response))
        //        {
        //            if (response.ErrorCode == ERROR_CODE.OK)
                    {
                        StoreData.ResetData();
                        //UpdateUserInfo(response.UInfo);

                        PhotonNetwork.Disconnect();
                        PopupEndGame.Create(resultChapter, 3);
                    }
            //        else
            //        {
            //            ProcessErrorResponse(response);
            //        }
            //    }
            //}, true,
            //false);
    }
}
