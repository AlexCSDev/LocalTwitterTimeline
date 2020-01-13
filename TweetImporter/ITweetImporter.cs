using System.Collections.Generic;
using System.Threading.Tasks;
using TweetDatabase.Models;

namespace TweetImporter
{
    public interface ITweetImporter
    {
        Task<List<Tweet>> ImportTweets(string[] jsonLines);
    }
}