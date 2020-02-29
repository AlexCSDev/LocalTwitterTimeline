using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using TweetDatabase.Models;

namespace WebFrontend.Services
{
    public class TweetService
    {
        private readonly IMongoCollection<dynamic> _tweets;
        public TweetService()
        {
            MongoClient client = new MongoClient("mongodb://127.0.0.1:27017");
            IMongoDatabase db = client.GetDatabase("localtwittertimeline");
            _tweets = db.GetCollection<dynamic>("tweets");
        }

        private bool DynamicContains(dynamic variable, string key)
        {
            return ((IDictionary<string, object>)variable).ContainsKey(key);
        }

        private Tweet BuildTweet(dynamic inputTweet)
        {
            Tweet tweet = new Tweet();
            if (DynamicContains(inputTweet, "retweeted_status"))
            {
                tweet.RetweetTweet = BuildTweet(inputTweet.retweeted_status);
            }

            if (DynamicContains(inputTweet, "quoted_status"))
            {
                tweet.QuotedTweet = BuildTweet(inputTweet.quoted_status);
            }

            tweet.Id = inputTweet.id;
            tweet.CreatedAt = DateTime.ParseExact(inputTweet.created_at,
                "ddd MMM dd HH:mm:ss +ffff yyyy", new System.Globalization.CultureInfo("en-US"));
            if (inputTweet.truncated)
                tweet.Text = inputTweet.extended_tweet.full_text;
            else
                tweet.Text = inputTweet.full_text;

            tweet.User = new User
            {
                Name = inputTweet.user.name,
                ScreenName = inputTweet.user.screen_name,
                ProfileImageUrl = inputTweet.user.profile_image_url
            };

            if (DynamicContains(inputTweet, "extended_entities") && DynamicContains(inputTweet.extended_entities, "media"))
            {
                tweet.Media = new List<Media>(4);
                foreach (dynamic dbMedia in inputTweet.extended_entities.media)
                {
                    Media media = new Media
                    {
                        Url = $"{dbMedia.media_url_https}:orig"
                    };

                    switch (dbMedia.type)
                    {
                        case "animated_gif":
                            media.MediaType = MediaType.AnimatedGIF;
                            break;
                        case "video":
                            media.MediaType = MediaType.Video;
                            break;
                        default:
                            media.MediaType = MediaType.Photo;
                            break;
                    }

                    tweet.Media.Add(media);
                }
            }

            return tweet;
        }

        public async Task<List<Tweet>> GetFrom(long id)
        {
            var results = await _tweets.Find(new BsonDocument("id", new BsonDocument("$lte", id)))
                .Sort("{id: -1}")
                .Limit(100)
                .ToListAsync();

            List<Tweet> tweets = new List<Tweet>(100);
            foreach (dynamic dbTweet in results)
            {
                Tweet tweet = BuildTweet(dbTweet);

                tweets.Add(tweet);
            }

            return tweets;
        }
    }
}
