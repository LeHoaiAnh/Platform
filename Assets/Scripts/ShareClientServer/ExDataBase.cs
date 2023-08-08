using System.Collections;
using System.Collections.Generic;

namespace Hiker.Networks.Data
{
    public enum ERROR_CODE
    {
        OK,
        UNKNOW_ERROR,
        INVALIDATE_ACCESS_TOKEN,
        DISPLAY_MESSAGE,
        DISPLAY_MESSAGE_AND_QUIT,
        CONFIG_VERSION_INVALID,
        GAME_VERSION_INVALID,
        IAP_PACKAGE_NOT_TRUE,
        IAP_TRANSACTION_EXISTED,
        INVALID_REQUEST,
        NOT_FINISHED_BATTLE,
        INVALID_INSTALL,
        EXCEED_TRIES,
        WAI_TO_FLUSH_REQUEST_QUEUE,
        OVERTIME_TO_JOIN_LEO_THAP,
        OVERTIME_TO_PLAY_LEO_THAP,
        BATTLE_IS_NOT_RECOGNIZED,
        NOT_ENOUGH_MONEY,
        //USER_NAME_NOT_EXIST,
        //USER_NAME_EXIST,
        //USER_NAME_INVALID,
        //USER_NAME_CHANGED,
    }

    public class ExDataBase
    {
        public string ErrorMessage { get; set; }              
        public ERROR_CODE ErrorCode { get; set; }

        public ExDataBase()
        {
            ErrorCode = ERROR_CODE.OK;
        }
    }

    public class SetTaiNguyenRequest : GIDRequest
    {
        public long gold    = -1;
        public int  gem     = -1;
        public int  theLuc  = -1;
        public int  shard   = -1;
        public long rune = -1;
        public int heroEXP = 0;
    }

    public class GIDRequest
    {
        public long GID;
        public string token;
        public long rqTime;
        public int ver;
        public string configVer;
        public string platform;
        public string lang;

        public void UpdateGIDData()
        {
#if !SERVER_CODE
            GID = GameClient.instance.GID;
            token = GameClient.instance.AccessToken != null ? GameClient.instance.AccessToken : "";
#else
            // GID = GameRequests.MASTER_CLIENT_ID;
            // token = GameRequests.MASTER_CLIENT_TOKEN;
#endif
            rqTime = TimeUtils.Now.Ticks;
        }
    }
}
