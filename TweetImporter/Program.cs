using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using NLog;
using TweetDatabase;
using TweetDatabase.Models;

namespace TweetImporter
{
    class Program
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        static async Task Main(string[] args)
        {
            NLogManager.ReconfigureNLog(true);

            _logger.Info("TweetImporter started");
            if (args.Length == 0 || !File.Exists(args[0]))
            {
                _logger.Fatal("File not found or not specified");
                return;
            }

            _logger.Debug("Initializing database");
            MongoClient client = new MongoClient("mongodb://127.0.0.1:27017");
            IMongoDatabase db = client.GetDatabase("localtwittertimeline");
            var tweets = db.GetCollection<BsonDocument>("tweets");

            _logger.Debug("Initializing reading input file");
            var data = await File.ReadAllLinesAsync(args[0]);

            int importCnt = 0;

            foreach (string line in data)
            {
                BsonDocument document = BsonDocument.Parse(line);
                var search = await tweets.FindAsync(x => x["id_str"] == document["id_str"]);
                if (search.ToList().Count == 0)
                {
                    _logger.Info($"{document["id_str"]} has been imported");
                    await tweets.InsertOneAsync(document);
                    importCnt++;
                }
                else
                    _logger.Warn($"{document["id_str"]} already exists");
            }

            _logger.Debug("Removing input file");
            File.Delete(args[0]);

            _logger.Info($"Imported {importCnt} tweets");
        }
    }
}