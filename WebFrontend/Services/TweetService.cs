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
            MongoClient client = new MongoClient("mongodb://127.0.0.1:27017");
            IMongoDatabase db = client.GetDatabase("localtwittertimeline");
            _tweets = db.GetCollection<dynamic>("tweets");
        }

        private bool DynamicContains(dynamic variable, string key)
        {
            return ((IDictionary<string, object>)variable).ContainsKey(key);
        }

        public async Task<List<TwitterStatus>> GetFrom(long id, bool sortAsc = false)
        {
            var results = await _tweets.Find(new BsonDocument("id", new BsonDocument(sortAsc ? "$gte" : "$lte", id)))
                .Sort("{id: " + (sortAsc ? "1" : "-1") + "}")
                .Limit(500)
                .ToListAsync();

            List<TwitterStatus> tweets = new List<TwitterStatus>(500);
            foreach (dynamic dbTweet in results)
            {
                TwitterStatus tweet = await JsonSerializer.DeserializeAsync<TwitterStatus>(new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(dbTweet)))).ConfigureAwait(false);

                tweet.ParsedFullText = TweetTextParser.ParseTweetText(tweet);
                if(tweet.OriginatingStatus != null)
                    tweet.OriginatingStatus.ParsedFullText = TweetTextParser.ParseTweetText(tweet.OriginatingStatus);
                if(tweet.QuotedStatus != null)
                    tweet.QuotedStatus.ParsedFullText = TweetTextParser.ParseTweetText(tweet.QuotedStatus);

                tweets.Add(tweet);
            }

            return tweets;
        }

        public async Task<List<dynamic>> GetFromDynamic(long id, bool sortAsc = false)
        {
            var results = await _tweets.Find(new BsonDocument("id", new BsonDocument(sortAsc ? "$gte" : "$lte", id)))
                .Sort("{id: " + (sortAsc ? "1" : "-1") + "}")
                .Limit(500)
                .ToListAsync();

            return results;
        }
    }
}
