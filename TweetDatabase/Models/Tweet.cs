#nullable enable
using System;
using System.Collections.Generic;

namespace TweetDatabase.Models
{
    public class Tweet
    {
        public long Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ImportedAt { get; set; }
        public User User { get; set; }
        public string Text { get; set; }
        public List<Media> Media { get; set; }
    }
}