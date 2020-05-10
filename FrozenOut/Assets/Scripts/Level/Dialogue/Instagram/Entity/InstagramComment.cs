using System.Collections.Generic;

namespace Scripts.Level.Dialogue.Instagram.Entity
{
    public class InstagramCommentData
    {
        public InstagramCommentData()
        {
            Data = new List<InstagramComment>();
        }

        public List<InstagramComment> Data
        {
            get;
            set;
        }
    }

    public class InstagramComment
    {
        public InstagramComment()
        {
        }

        public string Id
        {
            get;
            set;
        }
        public string Username
        {
            get;
            set;
        }
        public string Text
        {
            get;
            set;
        }
        public string Timestamp
        {
            get;
            set;
        }
        public string Like_count
        {
            get;
            set;
        }
    }
}