using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
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
        public PersonalPlayersStatController(StorageOfPersonalStat personalStatStorage, JsonWorker<PlayerPersonalStat> jsonPlayerWorker)
        {
            _personalStatStorage = personalStatStorage;
            _jsonPlayerWorker = jsonPlayerWorker;
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
            
            _jsonPlayerWorker.UpdateFile("PlayerPersonalStat.json",_personalStatStorage.GetDictionary());
            return Ok();
        }

        
    }
}
