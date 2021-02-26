using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using The_Game.Models;
using The_Game.Services;

namespace The_Game.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonalPlayersStatController : ControllerBase
    {
        private readonly JsonWorker<PlayerPersonalStat> _jsonPlayerWorker;
        private readonly StorageOfPersonalStat _personalStatStorage;
        private ILogger _logger;
        public PersonalPlayersStatController(StorageOfPersonalStat personalStatStorage, JsonWorker<PlayerPersonalStat> jsonPlayerWorker, ILogger<PersonalPlayersStatController> logger)
        {
            _personalStatStorage = personalStatStorage;
            _jsonPlayerWorker = jsonPlayerWorker;
            _logger = logger;
        }

        [HttpGet("{playerLogin}")]
        public ActionResult<PlayerPersonalStat> GetPersonalStat(string playerLogin)
        {
            
            return _personalStatStorage.GetDictionary().FirstOrDefault(x => x.Value.Login == playerLogin).Value;

        }

        [HttpPost]
        public  async  Task<IActionResult> PostPersonalStat(PlayerPersonalStat playerStat)
        { 
            await _personalStatStorage.UpdateStatAsync(playerStat);
            _logger.LogInformation($"stat is update for player {playerStat} ");
            _jsonPlayerWorker.UpdateFile("PlayerPersonalStat.json",_personalStatStorage.GetDictionary());
            return Ok();
        }

        
    }
}
