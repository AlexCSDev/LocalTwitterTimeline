using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using NLog.Fluent;
using TweetDatabase;
using TweetDatabase.Models;
using TweetImporter.TwitterJSON;
using Tweet = TweetDatabase.Models.Tweet;
using JSONTweet = TweetImporter.TwitterJSON.Tweet;
using JSONMediaType = TweetImporter.TwitterJSON.MediaType;
using MediaType = TweetDatabase.Models.MediaType;
using User = TweetDatabase.Models.User;

namespace TweetImporter
{
    public class TweetImporter : ITweetImporter
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        //TODO: RETWEETS
        public async Task<List<Tweet>> ImportTweets(string[] jsonLines)
        {
            _logger.Info("Started importing tweets");
            
            _logger.Debug("Forming valid json");
            StringBuilder stringBuilder = new StringBuilder();
            
            stringBuilder.Append("[");
            foreach (string line in jsonLines)
            {
                stringBuilder.Append(line);
                stringBuilder.Append(",");
            }
            stringBuilder.Append("]");
            
            _logger.Debug("Parsing json");
            List<JSONTweet> jsonTweets = TwitterJSON.Tweet.FromJson(stringBuilder.ToString());
            
            List<Tweet> returnList = new List<Tweet>();

            _logger.Debug("Creating db context");
            using (TweetDbContext dbContext = new TweetDbContext())
            {
                _logger.Debug($"Parsing users");
                List<User> users = new List<User>();
                foreach (JSONTweet jsonTweet in jsonTweets)
                {
                    _logger.Debug($"Parsing user in: {jsonTweet.Id}");
                    if (users.Any(u => u.OriginalId == jsonTweet.User.Id))
                    {
                        _logger.Debug($"User exists");
                        continue;
                    }
                    
                    User user = dbContext.Users.FirstOrDefault(u => u.OriginalId == jsonTweet.User.Id);

                    if (user == null)
                    {
                        _logger.Debug($"DB: Not found");
                        user = new User
                        {
                            OriginalId = jsonTweet.User.Id,
                            Name = jsonTweet.User.Name,
                            ScreenName = jsonTweet.User.ScreenName
                        };
                                            
                        users.Add(user);
                    }
                }
                _logger.Debug($"Saving db on users");
                await dbContext.AddRangeAsync(users);
                await dbContext.SaveChangesAsync();

                foreach (JSONTweet jsonTweet in jsonTweets)
                {
                    _logger.Debug($"Parsing {jsonTweet.Id}");
                    if (dbContext.Tweets.Any(t => t.OriginalId == jsonTweet.Id))
                        continue;
                    
                    _logger.Debug($"Does not exist");

                    Tweet dbTweet = new Tweet
                    {
                        OriginalId = jsonTweet.Id,
                        CreatedAt = DateTime.ParseExact(jsonTweet.CreatedAt,
                            "ddd MMM dd HH:mm:ss +ffff yyyy", new System.Globalization.CultureInfo("en-US")),
                        ImportedAt = DateTime.UtcNow
                    };

                    _logger.Debug($"Searching for user");
                    User user = dbContext.Users.FirstOrDefault(u => u.OriginalId == jsonTweet.User.Id);

                    if (user == null)
                    {
                        _logger.Fatal($"User not found");
                        throw new Exception("User not found");
                    }

                    dbTweet.User = user;

                    _logger.Debug($"Writing tweet text");
                    if (jsonTweet.Truncated)
                        dbTweet.Text = jsonTweet.ExtendedTweet.FullText;
                    else
                        dbTweet.Text = jsonTweet.FullText;

                    _logger.Debug($"Saving tweet");
                    returnList.Add(dbTweet);
                    dbContext.Add(dbTweet);
                }
                
                _logger.Debug($"Saving db on tweets...");
                await dbContext.SaveChangesAsync();
                
                _logger.Debug($"Parsing media");
                List<Media> media = new List<Media>();
                foreach (JSONTweet jsonTweet in jsonTweets)
                {
                    _logger.Debug($"Parsing media in: {jsonTweet.Id}");
                    if (jsonTweet.ExtendedEntities?.Media != null)
                    {
                        _logger.Debug($"Media found");
                        foreach (PurpleMedia jsonMedia in jsonTweet.ExtendedEntities.Media)
                        {
                            if (media.Any(m => m.OriginalId == jsonMedia.Id))
                            {
                                _logger.Debug($"Media exists");
                                continue;
                            }
                                
                            _logger.Debug($"Searching for media");
                            Media dbMedia = dbContext.Media.FirstOrDefault(m => m.OriginalId == jsonMedia.Id);

                            if (dbMedia == null)
                            {
                                    _logger.Debug($"Not Found, Creating...");
                                    _logger.Debug($"Searching tweet...");
                                    Tweet dbTweet = dbContext.Tweets.FirstOrDefault(t => t.OriginalId == jsonTweet.Id);

                                    if (dbTweet == null)
                                    {
                                        _logger.Fatal($"Tweet not found");
                                        throw new Exception("Tweet not found");
                                    }
                                    
                                    dbMedia = new Media
                                    {
                                        OriginalId = jsonMedia.Id,
                                        Url = $"{jsonMedia.MediaUrlHttps.ToString()}:orig",
                                        TweetId = dbTweet.Id
                                    };

                                    switch (jsonMedia.Type)
                                    {
                                        case JSONMediaType.AnimatedGif:
                                            dbMedia.MediaType = MediaType.AnimatedGIF;
                                            break;
                                        case JSONMediaType.Video:
                                            dbMedia.MediaType = MediaType.Video;
                                            break;
                                        default:
                                            dbMedia.MediaType = MediaType.Photo;
                                            break;
                                    }
                                    media.Add(dbMedia);
                            }
                        }
                    }
                }
                _logger.Debug($"Saving db on media");
                await dbContext.AddRangeAsync(media);
                await dbContext.SaveChangesAsync();
            }

            return returnList;
        }
    }
}