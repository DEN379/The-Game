﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using The_Game.Classes;
using Microsoft.Extensions.Logging;
using The_Game.Models;
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
                _logger.LogInformation($"Some one try to register with registered login {user.Login}");
                return BadRequest();
            }
            await _users.AddAsync(user);
            await _leaderboard.AddAsync(new Leaderboard()
            {
                Login = user.Login
            });
            _readLeaderboard.UpdateFile("Leaderboard.json",_leaderboard.GetDictionary());
            _readUsers.UpdateFile("Users.json", _users.GetDictionary());
            _logger.LogInformation($"Registered new user {user.Login}");
            return Ok();

        }
    }
}