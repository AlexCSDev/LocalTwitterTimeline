using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace TweetDatabase.Models
{
    public class UserObjectUrls
    {
        [JsonPropertyName("urls")]
        public UrlEntity[]? Urls { get; set; }
    }
}
