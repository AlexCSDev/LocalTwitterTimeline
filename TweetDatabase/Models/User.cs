using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.Json.Serialization;

namespace TweetDatabase.Models
{
    public class User
    {
        private string? _memberSince;

        [JsonPropertyName("id_str")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("screen_name")]
        public string? ScreenName { get; set; }

        [JsonPropertyName("profile_image_url_https")]
        public string? ProfileImageUrl { get; set; }

        [JsonPropertyName("profile_banner_url")]
        public string? ProfileBannerUrl { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("verified")]
        public bool Verified { get; set; }

        [JsonPropertyName("location")]
        public string? Location { get; set; }

        [JsonPropertyName("url")]
        public string? Url { get; set; }

        [JsonPropertyName("statuses_count")]
        public int Tweets { get; set; }

        [JsonPropertyName("friends_count")]
        public int Friends { get; set; }

        [JsonPropertyName("followers_count")]
        public int Followers { get; set; }

        [JsonPropertyName("entities")]
        public UserObjectEntities? Entities { get; set; }

        [JsonPropertyName("created_at")]
        public string? CreatedAt { get; set; }

        [JsonPropertyName("following")]
        public bool IsFollowing { get; set; }

        [JsonPropertyName("followed_by")]
        public bool IsFollowedBy { get; set; }

        [JsonIgnore]
        public string MemberSince
        {
            get
            {
                if (_memberSince is null)
                {
                    var date = TwitterStatus.ParseTwitterDate(CreatedAt);
                    _memberSince = date.ToString("MMM yyy", CultureInfo.InvariantCulture);
                }
                return _memberSince;
            }
        }

        [JsonIgnore]
        public string? ProfileImageUrlBigger => ProfileImageUrl?.Replace("_normal", "_bigger", StringComparison.Ordinal);

        [JsonIgnore]
        public string? ProfileImageUrlOriginal => ProfileImageUrl?.Replace("_normal", "", StringComparison.Ordinal);
    }
}