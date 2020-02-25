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
            if (args.Length == 0 || !File.Exists(args[0]))
            {
                _logger.Fatal("File not found or not specified");
                return;
            }
            _logger.Debug("Initializing database");
            using (TweetDbContext dbContext = new TweetDbContext())
            {
                dbContext.Database.Migrate();
            }
            _logger.Debug("Creating tweet parser");
            ITweetImporter tweetImporter = new TweetImporter();
            List<Tweet> tweets = await tweetImporter.ImportTweets(await File.ReadAllLinesAsync(args[0]));
            File.Delete(args[0]);
            _logger.Info($"Imported {tweets.Count} tweets");
        }
    }
}