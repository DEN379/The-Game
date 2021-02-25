using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace The_Game.Models
{
    public class PlayerPersonalStat
    {
        public string Login { get; set; }
        public int Wins { get; set; }
        public int Loses { get; set; }
        public int Draws { get; set; } 
        public float WinRate { get; set; }
        public int ScissorsCount { get; set; }
        public int RockCount { get; set; }
        public int PaperCount { get; set; }

        public Dictionary<DateTime, float> ChangesWinrate;
        // if currentDateTime = ChangesWinragte.key {value = }
        public TimeSpan TimeInGame { get; set; }

    }
    //private readonly JsonWorker<PlayerPersonalStat> _jsonPlayerWorker;
    //    private readonly StorageOfPersonalStat _personalStatStorage;

    //    public PersonalPlayersStatController(StorageOfPersonalStat personalStatStorage,
    //        JsonWorker<PlayerPersonalStat> jsonPlayerWorker)
    //    {
    //        _personalStatStorage = personalStatStorage;
    //        _jsonPlayerWorker = jsonPlayerWorker;
    //    }

    //    [HttpGet("/{playerLogin}")]
    //    public ActionResult<PlayerPersonalStat> GetPersonalStat(string playerLogin)
    //    {

    //        return _personalStatStorage.GetDictionary().FirstOrDefault(x => x.Value.Login == playerLogin).Value;

    //    }

    //    [HttpPost()]
    //    public async Task<IActionResult> PostPersonalStat(PlayerPersonalStat playerStat)
    //    {
    //        await _personalStatStorage.AddAsync(playerStat);
    //        _jsonPlayerWorker.UpdateFile("PlayerPersonalStat.json", _personalStatStorage.GetDictionary());
    //        return Ok();
    //    }
}
