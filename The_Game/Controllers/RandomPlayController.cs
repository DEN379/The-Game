using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using The_Game.Models;
using The_Game.Services;
using The_Game.Services.GameProcess;

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
            _logger.LogInformation($"{login} try to find a room for random play ");

            var room = _rooms.TakeLastRoom();
            if(room.Value==null)
            {
                var newRoom = new Room()
                {
                    Guid = room.Key,
                    Player1 = login,
                    Player2 = null
                };
                await _rooms.AddAsync(newRoom);
                _logger.LogInformation($"new room created by {login} he is waiting for player ");
                return newRoom.Guid;
            }

            room.Value.Player2 = login;
           await _session.AddWithGuidAsync(room.Value.Guid,room.Value);
           _logger.LogInformation($" {login} joined to {room.Value.Player1} , and they created a session ");

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
            _logger.LogInformation($"room with id {linkOfGuid} start there game");
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
                _logger.LogInformation($"{player.Login} make his turn, he put {player.Command}");
                return Ok();
            }

            room.SecondPlayer = player;
            await _playRooms.DeleteAsync(linkOfGuid);

            await _sessionPlayRooms.AddWithGuidAsync(linkOfGuid, room);

            _logger.LogInformation($"{player.Login} make his turn, he put {player.Command}");
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

            if (room.FirstPlayer == null)
            {
                await _sessionPlayRooms.DeleteAsync(linkOfGuid);
                await _playRooms.DeleteAsync(linkOfGuid);
                await _rooms.DeleteAsync(linkOfGuid);
                _jsonUpdaterLeaderBoard.UpdateFile("Leaderboard.json", _leaderBoard.GetDictionary());
                return "Exit";
            }
            var game = new GameProcess(room.FirstPlayer, room.SecondPlayer);
            var winner = await game.PlayersPlay();
            if (winner.Value == "Exit")
            {
                
                room.FirstPlayer = null;
                return "Exit";
            }

            _logger.LogInformation($"{winner.Value} Win");

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
