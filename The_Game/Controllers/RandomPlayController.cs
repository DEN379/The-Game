using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
        private readonly LeaderboardStorage _leaderBoard;
        private readonly JsonWorker<Leaderboard> _jsonUpdaterLeaderBoard;
        private readonly PlayRoomStorage _playRooms;
        private readonly PlaySessinRoomStorage _sessionPlayRooms;
        private readonly RoomStorage _session;

        

        public RandomPlayController(RoomStorage rooms, ILogger<RandomPlayController> logger, LeaderboardStorage leaderBoard, RoomStorage session, PlaySessinRoomStorage sessionplayRooms, PlayRoomStorage playRooms, JsonWorker<Leaderboard> jsonUpdaterLeaderBoard)
        {
            _rooms = rooms;
            _logger = logger;
            _leaderBoard = leaderBoard;
            _session = session;
            _sessionPlayRooms = sessionplayRooms;
            _playRooms = playRooms;
            _jsonUpdaterLeaderBoard = jsonUpdaterLeaderBoard;
        }

        [HttpGet("create/{login}")]
        public async  Task<ActionResult<Guid>> StartGame(string login)
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
                await _rooms.AddAsync(newRoom);
                return newRoom.Guid;
            }

            room.Value.Player2 = login;
           await _session.AddWithGuidAsync(room.Value.Guid,room.Value);
            
            await _rooms.DeleteAsync(room.Key);
            return room.Value.Guid;

        }

        [HttpGet("{linkOfGuid}")]
        public async Task<IActionResult> WaitingLobby(Guid linkOfGuid)
        {
            var room = await _session.Get(linkOfGuid);
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
          
           await _sessionPlayRooms.DeleteAsync(linkOfGuid);
            
            var room = await _playRooms.Get(linkOfGuid);
            if (room== null)
            {
                var newPlayRoom = new PlayRoom()
                {
                    FirstPlayer = player,
                    SecondPlayer = null
                };
                //
                await _playRooms.AddWithGuidAsync(linkOfGuid, newPlayRoom);
                return Ok();
            }

            room.SecondPlayer = player;
            await _playRooms.DeleteAsync(linkOfGuid);

            await _sessionPlayRooms.AddWithGuidAsync(linkOfGuid, room);
            return Ok();

            
        }
        [HttpGet("game/{linkOfGuid}")]
        public async Task<ActionResult<string>> WaitingEnemy(Guid linkOfGuid)
        {
           
           var room = await _sessionPlayRooms.Get(linkOfGuid);
            if (room == null)
            {
                return NotFound();
            }
            var game = new GameProcess(room.FirstPlayer,room.SecondPlayer);
            var winner = await game.PlayersPlay();
            if (winner.Value == "Exit")
            {
                _jsonUpdaterLeaderBoard.UpdateFile("Leaderboard.json",_leaderBoard.GetDictionary());
                return "Exit";
            }

            if (winner.Value == room.FirstPlayer.Login)
            {
               await _leaderBoard.AddWins(room.FirstPlayer.Login);
               await _leaderBoard.AddLoses(room.SecondPlayer.Login);
            }
            else if (winner.Value == room.SecondPlayer.Login)
            {
                await _leaderBoard.AddWins(room.SecondPlayer.Login);
                await _leaderBoard.AddLoses(room.FirstPlayer.Login);
            }
            if (winner.Value == "Draw")
            {
                await _leaderBoard.AddDraws(room.FirstPlayer.Login);
                await _leaderBoard.AddDraws(room.SecondPlayer.Login);
            }

            return winner;
        }
    }
}
