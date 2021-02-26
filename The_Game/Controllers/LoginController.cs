using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using The_Game.Classes;
using The_Game.Services;

namespace The_Game.Controllers
{
    
    [ApiController]
    [Route("[controller]")]
    public class LoginController : Controller
    {
        
            private readonly UserStorage _users;
            private readonly ILogger<LoginController> _logger;

            public LoginController(UserStorage users,ILogger<LoginController> regLogger)
            {
                _users = users;
                _logger = regLogger;

            }

            [HttpPost]
            public  IActionResult LoginUser(User user)
            {
                if (_users.FindUser(user))
                {
                    _logger.LogInformation($"User {user.Login} join ");
                    return Ok();
                }
                
                return BadRequest();

            }
    }
}
