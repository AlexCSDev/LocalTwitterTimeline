using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace TweetDatabase.Models
{
    public class Variant
    {
        [JsonPropertyName("url")]
        public string? Url { get; set; }
    }
}
