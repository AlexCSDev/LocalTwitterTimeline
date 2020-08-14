using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace TweetDatabase.Models
{
    public class UrlEntity
    {
        [JsonPropertyName("url")]
        public string Url { get; set; }

        [JsonPropertyName("display_url")]
        public string DisplayUrl { get; set; }

        [JsonPropertyName("expanded_url")]
        public string ExpandedUrl { get; set; }

        [JsonPropertyName("indices")]
        public int[] Indices { get; set; }
    }
}
