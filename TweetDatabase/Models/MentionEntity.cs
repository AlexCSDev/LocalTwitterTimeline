using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace TweetDatabase.Models
{
    public class MentionEntity
    {
        [JsonPropertyName("id_str")]
        public string Id { get; set; }

        [JsonPropertyName("screen_name")]
        public string ScreenName { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("indices")]
        public int[] Indices { get; set; }
    }
}
