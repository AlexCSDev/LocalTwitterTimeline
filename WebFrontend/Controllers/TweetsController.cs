using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TweetDatabase;
using TweetDatabase.Models;

namespace WebFrontend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TweetsController : ControllerBase
    {
        private readonly ILogger<TweetsController> _logger;

        private readonly TweetDbContext _dbContext;
        
        public TweetsController(ILogger<TweetsController> logger, TweetDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }
        
        // GET
        [HttpGet("{id?}")]
        public async Task<IActionResult> Get(long id = -1)
        {
            Object[] tweets = null;
            try
            {
                tweets = await _dbContext.Tweets.Include(t => t.Media).Include(t => t.User)
                    //.OrderByDescending(t=>t.CreatedAt)
                    .OrderByDescending(t=>t.OriginalId)
                    .Where(t => t.OriginalId <= (id != -1 ? id : Int64.MaxValue))
                    .Take(100)
                    .AsNoTracking()
                    .ToArrayAsync();
            }
            catch (Exception ex)
            {
                return new JsonResult(new {Code = 503, Message = "Exception has occured", Data = ex});
            }

            if (tweets.Length == 0)
            {
                return new JsonResult(new {Code = 404, Message = "No tweets found"});
            }

            long cursor = ((Tweet) tweets.Last()).OriginalId - 1;
            return new JsonResult(new {Code = 200, Message = "OK", Data = tweets, Cursor = cursor});
        }
    }
}