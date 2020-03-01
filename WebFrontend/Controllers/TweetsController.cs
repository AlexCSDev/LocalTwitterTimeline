using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using TweetDatabase;
using TweetDatabase.Models;
using WebFrontend.Services;

namespace WebFrontend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TweetsController : ControllerBase
    {
        private readonly ILogger<TweetsController> _logger;

        private readonly TweetService _tweetService;
        
        public TweetsController(ILogger<TweetsController> logger, TweetService tweetService)
        {
            _logger = logger;
            _tweetService = tweetService;
        }
        
        // GET
        [HttpGet("{id?}/{sortType?}")]
        public async Task<IActionResult> Get(long id = -1, string sortType = "desc")
        {
            bool sortAsc = sortType.ToLower() == "asc";
            List<Tweet> tweets = null;
            try
            {
                tweets = await _tweetService.GetFrom(id > 0 ? id : (sortAsc ? Int64.MinValue : Int64.MaxValue), sortAsc);
            }
            catch (Exception ex)
            {
                return new JsonResult(new {Code = 503, Message = "Exception has occured", Data = ex});
            }

            if (tweets.Count == 0)
            {
                return new JsonResult(new {Code = 404, Message = "No tweets found"});
            }

            long cursor = tweets.Last().Id;
            if (sortAsc)
                cursor += 1;
            else
                cursor -= 1;

            return new JsonResult(new {Code = 200, Message = "OK", Data = tweets, Cursor = cursor});
        }
    }
}