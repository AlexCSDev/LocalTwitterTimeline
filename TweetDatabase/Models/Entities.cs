﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;

namespace TweetDatabase.Models
{
    public class Entities
    {
        [JsonPropertyName("urls")]
        public UrlEntity[]? Urls { get; set; }

        [JsonPropertyName("user_mentions")]
        public MentionEntity[]? Mentions { get; set; }

        [JsonPropertyName("hashtags")]
        public HashTagEntity[]? HashTags { get; set; }

        [JsonPropertyName("media")]
        public Media[]? Media { get; set; }

        [JsonIgnore]
        public bool HasMedia => Media != null && Media.Any(media => !string.IsNullOrWhiteSpace(media.MediaUrl));
    }
}
