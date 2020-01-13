using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TweetDatabase.Models
{
    public class User
    {
        public long Id { get; set; }
        public long OriginalId { get; set; }
        public string Name { get; set; }
        public string ScreenName { get; set; }
        public List<Tweet> Tweets { get; set; }
    }
}