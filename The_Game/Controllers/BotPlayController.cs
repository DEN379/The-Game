using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using The_Game.Models;
using The_Game.Services;

namespace The_Game.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BotPlayController : ControllerBase
    {

        private Player _bot = new Player()
            {Login = "Bot"};

        [HttpPost]
        public async Task<ActionResult<string>> PlayWithBot(Player player)
        {
            Random rand = new Random();
            _bot.Command = (Commands) rand.Next(1, 3);
            var game = new GameProcess(player, _bot);
           var winner=await game.PlayersPlay();
           return winner.Value == "Exit" ? "Exit" : winner;
        }
    }
}
