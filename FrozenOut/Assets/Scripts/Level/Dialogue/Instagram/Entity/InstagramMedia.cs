using System.Collections.Generic;

namespace Scripts.Level.Dialogue.Instagram.Entity
{
    public class InstagramMediaData
    {
        public InstagramMediaData()
        {
            Data = new List<InstagramMedia>();
        }

        public List<InstagramMedia> Data
        {
            get;
            set;
        }
    }

    public class InstagramMedia
    {
        public InstagramMedia()
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
        public string Caption
        {
            get;
            set;
        }
        public InstagramCommentData Comments
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
        public string Comments_count
        {
            get;
            set;
        }
    }
}