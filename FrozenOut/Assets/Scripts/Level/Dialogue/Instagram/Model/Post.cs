﻿using System;
using System.Collections.Generic;

namespace Scripts.Level.Dialogue.Instagram.Model
{
    public class Post
    {
        public Post()
        {
            Comments = new List<Comment>();
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
        public ICollection<Comment> Comments
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