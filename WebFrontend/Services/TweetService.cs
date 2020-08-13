using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using TweetDatabase.Models;
using WebFrontend.Helpers;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace WebFrontend.Services
{
    public class TweetService
    {
        private readonly IMongoCollection<dynamic> _tweets;
        public TweetService()
        {
            //TODO: move mongo settings to config file
            MongoClient client = new MongoClient("mongodb://127.0.0.1:27017");
            IMongoDatabase db = client.GetDatabase("localtwittertimeline");
            _tweets = db.GetCollection<dynamic>("tweets");
        }

        public async Task<List<TwitterStatus>> GetFrom(long id, bool sortAsc = false, int count = 500)
        {
            var results = await _tweets.Find(new BsonDocument("id", new BsonDocument(sortAsc ? "$gte" : "$lte", id)))
                .Sort("{id: " + (sortAsc ? "1" : "-1") + "}")
                .Limit(count)
                .ToListAsync();

            List<TwitterStatus> tweets = new List<TwitterStatus>(count);
            foreach (dynamic dbTweet in results)
            {
                using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(dbTweet))))
                {
                    TwitterStatus tweet = await JsonSerializer.DeserializeAsync<TwitterStatus>(ms).ConfigureAwait(false);

                    //Fill parsed text variables
                    tweet.ParsedFullText = TweetTextParser.ParseTweetText(tweet);
                    if (tweet.OriginatingStatus != null)
                        tweet.OriginatingStatus.ParsedFullText = TweetTextParser.ParseTweetText(tweet.OriginatingStatus);
                    if (tweet.QuotedStatus != null)
                        tweet.QuotedStatus.ParsedFullText = TweetTextParser.ParseTweetText(tweet.QuotedStatus);

                    tweets.Add(tweet);
                }
            }

            return tweets;
        }
    }
}
