using System.Collections.Generic;

namespace Scripts.Level.Dialogue.Instagram.Entity
{
    public class InstagramMedia
    {
        public InstagramMedia()
        {
            Comments = new List<InstagramComment>();
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
        public ICollection<InstagramComment> Comments
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
        public string Comment_count
        {
            get;
            set;
        }
    }
}