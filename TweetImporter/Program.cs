using System;
using System.IO;

namespace TweetImporter
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var tweets = TwitterJSON.Tweet.FromJson(File.ReadAllText("test.json"));
            Console.WriteLine($"tweets: {tweets.Count}");
        }
    }
}