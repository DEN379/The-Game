using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using The_Game.Services;

namespace The_Game.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaderboardController : ControllerBase
    {
        private LeaderboardStorage _leaderboard;
        private ILogger _logger;
        public LeaderboardController(LeaderboardStorage leaderboard, ILogger<LeaderboardController> logger)
        {
            _leaderboard = leaderboard;
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<string> ShowLeaderBoard()
        {
            _logger.LogInformation("Someone asked to take leaderboard");
            string board = "PlayerLogin : Wins : Losses : Draws : Total \n";
            var arrayOfLeaders = _leaderboard.GetDictionary().Select(x => x.Value).Where(x => x.Total >= 10).ToArray();
            if (arrayOfLeaders.Length <= 0)
            {
                return "No leaders yet, you can be the first";
            }
            foreach (var item in arrayOfLeaders)
            {
                board += $"{item.Login} : {item.Wins} : {item.Loses} : {item.Draws} : {item.Total}\n";
            }
            _logger.LogInformation(" Leaderboard is shown ");

            return board;

        }
    }
}
