using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using The_Game.Models;
using The_Game.Services;

namespace The_Game.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RandomPlayController : ControllerBase
    {
        private readonly RoomStorage _rooms;
        private readonly ILogger _logger;
        private static readonly ConcurrentDictionary<Guid, Room> Session = new ConcurrentDictionary<Guid, Room>();
        private static readonly ConcurrentDictionary<Guid,PlayRoom> PlayRooms= new ConcurrentDictionary<Guid, PlayRoom>();
        private static readonly ConcurrentDictionary<Guid, PlayRoom> SessionPlayRooms = new ConcurrentDictionary<Guid, PlayRoom>();

        public RandomPlayController(RoomStorage rooms, ILogger<RandomPlayController> logger)
        {
            _rooms = rooms;
            _logger = logger;
            
        }

        [HttpGet("create/{login}")]
        public ActionResult<Guid> StartGame(string login)
        {
            var room = _rooms.TakeLastRoom();
            if(room.Value==null)
            {
                var newRoom = new Room()
                {
                    Guid = Guid.NewGuid(),
                    Player1 = login,
                    Player2 = null
                };
                _rooms.AddAsync(newRoom);
                return newRoom.Guid;
            }

            room.Value.Player2 = login;
            Session.TryAdd(room.Value.Guid, room.Value);
            _rooms.DeleteAsync(room.Key);
            return room.Value.Guid;

        }

        [HttpGet("{linkOfGuid}")]
        public IActionResult WaitingLobby(Guid linkOfGuid)
        {
            var room = Session.Select(x => x).FirstOrDefault(x => x.Key == linkOfGuid).Value;
            if (room == null)
            {
                return NotFound();
            }
            else
            {
                return Ok();
            }
        }

        [HttpPost("{linkOfGuid}")]
        public async Task<IActionResult> PlayGameAsync(Guid linkOfGuid,Player player)
        {

            var room = PlayRooms.Select(x => x).FirstOrDefault(x => x.Key == linkOfGuid).Value;
            if (room== null)
            {
                var newPlayRoom = new PlayRoom()
                {
                    FirstPlayer = player,
                    SecondPlayer = null
                };
                PlayRooms.TryAdd(linkOfGuid,newPlayRoom);
                return Ok();
            }

            room.SecondPlayer = player;
            PlayRooms.TryRemove(linkOfGuid,out _);
            SessionPlayRooms.TryAdd(linkOfGuid, room);
            return Ok();

            #region NonWorkWariant

            //bool notStarted = true;
            //Player firstPlayer = null;
            //Player secondPlayer = null;
            //while (true)
            //{
            //    if (firstPlayer != null && secondPlayer != null)
            //    {
            //        break;
            //    }

            //    if (player.Login == room.Player1)
            //    {
            //        firstPlayer = player;
            //    }
            //    else
            //    {
            //        secondPlayer = player;
            //    }
            //}

            //var winner = await game.PlayersPlay(firstPlayer, secondPlayer);

            #endregion
        }
        [HttpGet("game/{linkOfGuid}")]
        public async Task<ActionResult<string>> WaitingEnemy(Guid linkOfGuid)
        {
            var room = SessionPlayRooms.Select(x => x).FirstOrDefault(x => x.Key == linkOfGuid).Value;
            if (room == null)
            {
                return NotFound();
            }
            var game = new GameProcess(room.FirstPlayer,room.FirstPlayer);
            var winner = await game.PlayersPlay();
            if (winner.Value == "Exit")
            {
                SessionPlayRooms.TryRemove(linkOfGuid, out _);
                return BadRequest();
            }

            return winner;
        }
    }
}
//Controller dlia privat commnat //
//Bot
//pochaty statistiky zapysuvaty
//