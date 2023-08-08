using System;

public interface IGameClient
{
    PlayerManagement playerManager { get; set; }
    SpriteCollection spriteCollection { get; }
    UserInfo UInfo { get; set; }

    UserInfo CreateNewUser();
    void LinkAccount(LoginType loginType, string socialID, string deviceID, string[] datas);
    void LoadChapter(int idx);
    void LoginApple(string appleID, string deviceID, string[] datas);
    void LoginDevice(string deviceID);
    void LoginGoogle(string googleID, string deviceID, string[] datas);
    void LoginOffline();
    void RequestAssetBundlesInfo(Action<string> onsuccess, Action<string> onfail);
    void RequestGetUserInfo(string[] props, bool isRunInBackground);
    void ShowConfigVersionUpdatePopup();
    void ShowGameVersionUpdadePopup(string url);
    void ShowInvalidInstallPopup(string url);
    void ShowPopupMessageAndQuit(string msg);
    bool VerifyEnoughGem(int gem, bool confirmToShop = false);
    bool VerifyEnoughGold(long gold, bool confirmToShop = false);
    bool VerifyEnoughRune(int rune);
    bool VerifyEnoughShard(int shard, bool confirmToShop = false);
}