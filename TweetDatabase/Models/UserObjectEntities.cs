using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace TweetDatabase.Models
{
    public class UserObjectEntities
    {
        [JsonPropertyName("url")]
        public UserObjectUrls? Url { get; set; }
    }
}
