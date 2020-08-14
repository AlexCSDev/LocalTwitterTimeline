using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace TweetDatabase.Models
{
    public class HashTagEntity
    {
        [JsonPropertyName("text")]
        public string Text { get; set; }

        [JsonPropertyName("indices")]
        public int[] Indices { get; set; }
    }
}
