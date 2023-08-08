using DateTime = System.DateTime;

namespace Hiker.Networks.Data
{
    public enum MailStatus
    {
        Unread,
        Read,
        Received
    }

    //public class MailInvite
    //{
    //    public long clanId;
    //    public string clanName;
    //}

    public class MailContent
    {
        public string text;
        public string link;
        //public MailInvite clan;
    }
    [System.Serializable]
    public class MailData
    {
        public string ID;
        public long GID;
        public string name;
        public long fromGID;
        public MailContent content;
        public MailStatus status;
        public DateTime sendTime = TimeUtils.Now;
        public int clientVersion;
    }
}