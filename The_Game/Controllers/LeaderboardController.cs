using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using The_Game.Services;

namespace The_Game.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaderboardController : ControllerBase
    {
        private LeaderboardStorage _leaderboard;

        public LeaderboardController(LeaderboardStorage leaderboard)
        {
            _leaderboard = leaderboard;
        }

        [HttpGet]
        public ActionResult<string> ShowLeaderBoard()
        {
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

            return board;

        }
    }
}
