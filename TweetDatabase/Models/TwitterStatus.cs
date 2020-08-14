#nullable enable
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TweetDatabase.Models
{
    public class TwitterStatus
    {
        private DateTime _createdDate;

        [JsonPropertyName("id_str")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("full_text")]
        public string? FullText { get; set; }

        [JsonPropertyName("text")]
        public string? Text { get; set; }

        [JsonPropertyName("user")]
        public User User { get; set; }

        [JsonPropertyName("created_at")]
        public string CreatedAt { get; set; }

        [JsonPropertyName("entities")]
        public Entities? Entities { get; set; }

        [JsonPropertyName("extended_entities")]
        public Entities? ExtendedEntities { get; set; }

        public bool IsQuoted => QuotedStatus != null;

        [JsonPropertyName("quoted_status")]
        public TwitterStatus? QuotedStatus { get; set; }

        [JsonPropertyName("quote_count")] 
        public int QuoteCount { get; set; }

        public bool IsRetweet => RetweetedStatus != null;

        [JsonPropertyName("retweeted_status")]
        public TwitterStatus? RetweetedStatus { get; set; }

        [JsonPropertyName("retweeted")] 
        public bool RetweetedByMe { get; set; }

        [JsonPropertyName("retweet_count")] 
        public int RetweetCount { get; set; }

        [JsonPropertyName("favorite_count")] 
        public int FavoriteCount { get; set; }

        [JsonPropertyName("favorited")] 
        public bool Favorited { get; set; }

        [JsonPropertyName("in_reply_to_status_id_str")]
        public string? InReplyToStatusId { get; set; }

        [JsonPropertyName("in_reply_to_user_id_str")]
        public string? InReplyToUserId { get; set; }

        [JsonPropertyName("in_reply_to_screen_name")]
        public string? InReplyToScreenName { get; set; }

        [JsonPropertyName("reply_count")] 
        public int ReplyCount { get; set; }

        [JsonPropertyName("possibly_sensitive")]
        public bool IsSensitive { get; set; }

        [JsonIgnore]
        public string? OverrideLink { get; set; }

        /// <summary>
        /// Originating status is what get's displayed
        /// </summary>
        [JsonIgnore]
        public TwitterStatus OriginatingStatus => IsRetweet
            ? RetweetedStatus ?? throw new InvalidOperationException("Invalid program state")
            : null;

        /// <summary>
        /// Create a link to a twitter status
        /// </summary>
        [JsonIgnore]
        public string StatusLink => string.IsNullOrWhiteSpace(OverrideLink)
            ? $"https://twitter.com/{User?.ScreenName}/status/{Id}"
            : OverrideLink;

        /// <summary>
        /// Converts a serialized twitter date into a System.DateTime object and caches it.
        /// </summary>
        [JsonIgnore]
        public DateTime CreatedDate
        {
            get
            {
                if (_createdDate == default)
                {
                    _createdDate = ParseTwitterDate(CreatedAt);
                }
                return _createdDate;
            }
        }

        /// <summary>
        /// Indicates if user is author of tweet
        /// </summary>
        public bool IsMyTweet { get; set; }

        public bool MentionsMe { get; set; }

        [JsonIgnore]
        public string ParsedFullText { get; set; }

        public static DateTime ParseTwitterDate(string? s)
        {
            return string.IsNullOrWhiteSpace(s)
                ? (default)
                : DateTime.ParseExact(
                    s,
                    "ddd MMM dd HH:mm:ss zzz yyyy",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.AdjustToUniversal);
        }
    }
}