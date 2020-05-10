using System.Collections.Generic;

namespace Scripts.Level.Dialogue.Instagram.Entity
{
    public class FacebookAccountData
    {
        public FacebookAccountData()
        {
            Data = new List<FacebookAccount>();
        }

        public List<FacebookAccount> Data
        {
            get;
            set;
        }
    }

    public class FacebookAccount
    {
        public FacebookAccount()
        {
        }

        public string Id
        {
            get;
            set;
        }
        public string Name
        {
            get;
            set;
        }
        public InstagramAccount Instagram_business_account
        {
            get;
            set;
        }
    }
}