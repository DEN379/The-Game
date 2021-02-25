using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using The_Game.Classes;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using The_Game.Interfaces;
using The_Game.Services;

namespace The_Game.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegistrationController : ControllerBase
    {
        private readonly JsonWorker<User> _readUsers = new JsonWorker<User>();
        private readonly JsonWorker<Leaderboard> _readLeaderboard = new JsonWorker<Leaderboard>();
        private  readonly UserStorage _users;
        private readonly ILogger<RegistrationController> _logger;
        private readonly LeaderboardStorage _leaderboard;

        public RegistrationController(UserStorage storage ,ILogger<RegistrationController> regLogger, LeaderboardStorage leaderboard)
        {
            _users = storage;
            _logger = regLogger;
            _leaderboard = leaderboard;
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUser(User user)
        {
            if (_users.FindUser(user))
            {
                return BadRequest();
            }
            await _users.AddAsync(user);
            await _leaderboard.AddAsync(new Leaderboard()
            {
                Login = user.Login
            });
            _readLeaderboard.UpdateFile("Leaderboard.json",_leaderboard.GetDictionary());
            _readUsers.UpdateFile("Users.json", _users.GetDictionary());
            return Ok();

        }
    }
}