using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using The_Game.Models;
using The_Game.Services.GameProcess;

namespace The_Game.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BotPlayController : ControllerBase
    {
        private ILogger _logger;

        private Player _bot = new Player() {Login = "Bot"};

        public BotPlayController(ILogger<BotPlayController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult<string>> PlayWithBot(Player player)
        {
            _logger.LogInformation($"{player.Login} training  with bot and put {player.Command} ");
            Random rand = new Random();
            _bot.Command = (Commands) rand.Next(1, 3);
            var game = new GameProcess(player, _bot);
           var winner=await game.PlayersPlay();
           _logger.LogInformation($"and {player.Login} vs Bot , winner is {winner.Value}  ");
            return winner.Value == "Exit" ? "Exit" : winner;
        }
    }
}
