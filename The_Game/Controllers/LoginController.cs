using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using The_Game.Classes;
using The_Game.Services;

namespace The_Game.Controllers
{
    
    [ApiController]
    [Route("[controller]")]
    public class LoginController : Controller
    {
        
        
            private readonly JsonWorker<User> _readUsers = new JsonWorker<User>();
            private readonly UserStorage _users;
            private readonly ILogger<LoginController> _logger;

            public LoginController(UserStorage users,ILogger<LoginController> regLogger)
            {
                _users = users;
                _logger = regLogger;

            }

            [HttpPost]
            public async Task<IActionResult> LoginUser(User user)
            {
                if (_users.FindUser(user))
                {
                    return Ok();
                }

                return BadRequest();

            }
    }
}
