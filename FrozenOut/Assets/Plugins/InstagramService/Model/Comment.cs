using System;

namespace InstagramConnection.Model
{
    public class Comment
    {
        public Comment()
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
        public DateTime Timestamp
        {
            get;
            set;
        }
        public int Likes
        {
            get;
            set;
        }
    }
}