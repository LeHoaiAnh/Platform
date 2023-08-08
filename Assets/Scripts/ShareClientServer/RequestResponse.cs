using System.Collections.Generic;
using DateTime = System.DateTime;

namespace Hiker.Networks.Data
{
    public class GetUserInfoRequest : GIDRequest
    {
        public string[] Props;
    }

    public class LoginRequest : GIDRequest
    {
        public string DeviceID;
        public string AppleID;
        public string GoogleID;
        public string[] Datas;
    }

    public class UserInfoResponse : ExDataBase
    {
        public UserInfo UInfo;
    }

    public class LoginResponse : UserInfoResponse
    {
        public string Token;
    }

    #region Inventory

    public class Inventory
    {
        public long GID;
        public List<InventoryInfor> inventoryInfors;
    }

    #endregion

    #region EquipItem

    public class EquipItemSlots
    {
        public long GID;
        public List<ItemInventoryInfor> equipSlots;
    }

    #endregion

    #region Chapter

    public class ChapterInfor
    {
        public long GID;
        public List<ChapterData> chapterDatas;
    }

    public class ChapterData
    {
        public int ChapIdx;
        public bool IsComplete = false;
        public float clearedTime;
        public int star;
    }

    #endregion

    public class BuyRequest : GIDRequest
    {
        public TypeItem typeItem;
        public string codename;
        public int quality;
    }

    public class BuyResponse : UserInfoResponse
    {
        public GeneralReward reward;
    }

    public class EquipItemRequest : GIDRequest
    {
        public TypeItem type;
        public string codename;
        public bool equip;
    }

    public class ForgeRequest : GIDRequest
    {
        public List<ForgeMaterial> materials;
    }

    public class ForgeResponse : UserInfoResponse
    {
        public GeneralReward reward;
    }

    public class ForgeMaterial
    {
        public ItemInventoryInfor infor;
        public int count;
    }

    public class ChapterStartRequest : GIDRequest
    {
        public int chapter;
    }

    public class ChapterStartResponse : UserInfoResponse
    {
        public PlayerStat PlayerUnit;
    }
    
    [System.Serializable]
    public class PlayerStat
    {
        #region Movement
        public float movementVelocity;
        public float jumpVelocity;
        public int jumpTimes;
        public float inAirJumpMultiplier;
        #endregion
    
        #region Energy
        public float Energy;
        public float consumeSpeed;
        public float armor;
        #endregion

        #region Attack

        public float atkSpeed;
        public float timeResetCombo;
        public long meleeDmg;
    
        #endregion

        public static PlayerStat Clone(PlayerStat old)
        {
            PlayerStat newPlayerStats = new PlayerStat();
            newPlayerStats.movementVelocity = old.movementVelocity;
            newPlayerStats.jumpVelocity = old.jumpVelocity;
            newPlayerStats.jumpTimes = old.jumpTimes;
            newPlayerStats.inAirJumpMultiplier = old.inAirJumpMultiplier;

            newPlayerStats.Energy = old.Energy;
            newPlayerStats.consumeSpeed = old.consumeSpeed;
            newPlayerStats.armor = old.armor;

            newPlayerStats.atkSpeed = old.atkSpeed;
            newPlayerStats.timeResetCombo = old.timeResetCombo;
            newPlayerStats.meleeDmg = old.meleeDmg;
            return newPlayerStats;
        }
    }
    
    public class ChapterEndRequest : GIDRequest
    {
        public ResultChapter resultChapter;
    }

    public class ChapterEndResponse : UserInfoResponse
    {
        public int star;
    }
    
    public enum ResultBattle
    {
        WIN,
        LOSE
    }
    
    public class ResultChapter
    {
        public int chapterIdx;
        public ResultBattle result;
        public int perCompleted;
        public float timeFinished;
        public int goldCollected;
    }

}
