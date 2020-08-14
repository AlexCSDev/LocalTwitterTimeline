using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace TweetDatabase.Models
{
    public class VideoInfo
    {
        [JsonPropertyName("variants")]
        public Variant[]? Variants { get; set; }
    }
}
