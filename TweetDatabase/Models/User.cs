using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TweetDatabase.Models
{
    public class User
    {
        public string Name { get; set; }
        public string ScreenName { get; set; }

        public string ProfileImageUrl { get; set; }
    }
}