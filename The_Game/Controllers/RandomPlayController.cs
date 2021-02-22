using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Concurrent;
using System.Linq;
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

        [HttpPost("{linkOfGuid}/{loginPlayer}")]
        public ActionResult PlayGame(Guid linkOfGuid,string loginPlayer, Figures figure)
        {
            var room = Session.Select(x => x).FirstOrDefault(x => x.Key == linkOfGuid).Value;
            bool notStarted = true;
            Figures? figureFirstPlayer = null;
            Figures? figureSecondPlayer = null;
            while (notStarted)
            {
                if (figureSecondPlayer != null && figureFirstPlayer != null)
                {
                    notStarted = false;
                }

                if (loginPlayer == room.Player1)
                {
                    figureFirstPlayer = figure;
                }
                else
                {
                    figureSecondPlayer = figure;
                }

                
            }

            return Ok();

        }
    }
}
