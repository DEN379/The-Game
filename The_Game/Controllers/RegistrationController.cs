using System;
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

namespace The_Game.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegistrationController : ControllerBase
    {
        private readonly JsonWorker<User> _readUsers = new JsonWorker<User>();
        private  readonly List<User> _users;
        private readonly ILogger<RegistrationController> _logger;

        public RegistrationController(ILogger<RegistrationController> regLogger)
        {
            {
                try
                {
                    _users = _readUsers.ReadList("Users.json").Result;
                }
                catch
                {
                    _users = new List<User>();
                }
            }

            _logger = regLogger;
            
        }
        //{
        
        //}
        [HttpGet]
        public async Task<IActionResult> GetRegistrarionInfo()
        {
            return Ok();
        }
        [HttpPost]
        public async Task<IActionResult> RegisterUser(User user)
        {
            var findUser = _users.FirstOrDefault(x => x.Login == user.Login);
            if (findUser != null)
            {
                return BadRequest();
            }
            _users.Add(user);
            _readUsers.UpdateFile("Users.json", _users);
            
            return Ok();

        }
    }
}