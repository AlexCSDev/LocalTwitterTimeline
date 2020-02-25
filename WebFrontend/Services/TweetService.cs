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

        public async Task<List<Tweet>> GetFrom(long id)
        {
            var results = await _tweets.Find(new BsonDocument("id", new BsonDocument("$lte", id)))
                .Sort("{id: -1}")
                .Limit(100)
                .ToListAsync();

            List<Tweet> tweets = new List<Tweet>(100);
            foreach (dynamic dbTweet in results)
            {
                Tweet tweet = new Tweet();
                tweet.Id = dbTweet.id;
                tweet.CreatedAt = DateTime.ParseExact(dbTweet.created_at,
                    "ddd MMM dd HH:mm:ss +ffff yyyy", new System.Globalization.CultureInfo("en-US"));
                if (dbTweet.truncated)
                    tweet.Text = dbTweet.extended_tweet.full_text;
                else
                    tweet.Text = dbTweet.full_text;

                tweet.User = new User
                {
                    Name = dbTweet.user.name,
                    ScreenName = dbTweet.user.screen_name
                };

                if (DynamicContains(dbTweet, "extended_tweet") && DynamicContains(dbTweet.extended_tweet,"media"))
                {
                    tweet.Media = new List<Media>(4);
                    foreach (dynamic dbMedia in dbTweet.extended_tweet.media)
                    {
                        Media media = new Media
                        {
                            Url = $"{dbMedia.MediaUrlHttps}:orig"
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

                tweets.Add(tweet);
            }

            return tweets;
        }
    }
}
