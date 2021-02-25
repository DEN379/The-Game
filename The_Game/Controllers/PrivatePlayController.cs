using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using The_Game.Models;
using The_Game.Services;

namespace The_Game.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrivatePlayController : ControllerBase
    {
        private readonly RoomStorage _rooms;
        private readonly ILogger _logger;
        private static readonly ConcurrentDictionary<Guid, Room> Session = new ConcurrentDictionary<Guid, Room>();
        private static readonly ConcurrentDictionary<Guid, PlayRoom> PlayRooms = new ConcurrentDictionary<Guid, PlayRoom>();
        private static readonly ConcurrentDictionary<Guid, PlayRoom> SessionPlayRooms = new ConcurrentDictionary<Guid, PlayRoom>();

        public PrivatePlayController(RoomStorage rooms, ILogger<PrivatePlayController> logger)
        {
            _rooms = rooms;
            _logger = logger;

        }

        [HttpGet("create/{login}")]
        public ActionResult<Guid> StartGame(string login)
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
        
        [HttpGet("{login}/{linkOfGuid}")]
        public async Task<IActionResult> WaitingLobby(string login,Guid linkOfGuid)
        {
            var waitingRoom = _rooms.SelectRoom(linkOfGuid);
            if (waitingRoom == 0)
            {
                return NotFound();
            }

            if (Session.TryGetValue(linkOfGuid, out var checkroom))
            {
                return Ok();
            }
            
            var newSessionRoom = await _rooms.Get(int.Parse(waitingRoom.ToString()));
            if (newSessionRoom.Player1 == login)
            {
                return NotFound();
            }
            
            newSessionRoom.Player2 = login;
            Session.TryAdd(newSessionRoom.Guid, newSessionRoom);
            //await _rooms.DeleteAsync(int.Parse(waitingRoom.ToString()));
            return Ok();
            
            
        }

        [HttpPost("{linkOfGuid}")]
        public async Task<IActionResult> PlayGameAsync(Guid linkOfGuid, Player player)
        {
            SessionPlayRooms.TryRemove(linkOfGuid, out _);
            var room = PlayRooms.Select(x => x).FirstOrDefault(x => x.Key == linkOfGuid).Value;
            if (room == null)
            {
                var newPlayRoom = new PlayRoom()
                {
                    FirstPlayer = player,
                    SecondPlayer = null
                };
                PlayRooms.TryAdd(linkOfGuid, newPlayRoom);
                return Ok();
            }

            room.SecondPlayer = player;
            PlayRooms.TryRemove(linkOfGuid, out _);
            SessionPlayRooms.TryAdd(linkOfGuid, room);
            return Ok();

            
        }
        [HttpGet("game/{linkOfGuid}")]
        public async Task<ActionResult<string>> WaitingEnemy(Guid linkOfGuid)
        {
            var room = SessionPlayRooms.Select(x => x).FirstOrDefault(x => x.Key == linkOfGuid).Value;
            if (room == null)
            {
                return NotFound();
            }
            var game = new GameProcess(room.FirstPlayer, room.SecondPlayer);
            var winner = await game.PlayersPlay();
            if (winner.Value == "Exit")
            {
                return "Exit";
            }


            return winner;
        }
    }
}
