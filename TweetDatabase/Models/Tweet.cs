#nullable enable
using System;
using System.Collections.Generic;

namespace TweetDatabase.Models
{
    public class Tweet
    {
        public long Id { get; set; }
        public long OriginalId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ImportedAt { get; set; }
        public long UserId { get; set; }
        public User User { get; set; }
        public string Text { get; set; }
        public List<Media> Media { get; set; }
        
        // FOR RETWEETS (AND REPLIES?)
        public long? RelatedTweetId { get; set; }
        public Tweet? RelatedTweet { get; set; }
    }
}