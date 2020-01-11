using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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
            _logger.Debug("Initializing database");
            using (TweetDbContext dbContext = new TweetDbContext())
            {
                dbContext.Database.Migrate();
            }
            _logger.Debug("Creating tweet parser");
            ITweetImporter tweetImporter = new TweetImporter();
            List<Tweet> tweets = await tweetImporter.ImportTweets(File.ReadAllText("test.json"));
            _logger.Info($"Imported {tweets.Count} tweets");
        }
    }
}