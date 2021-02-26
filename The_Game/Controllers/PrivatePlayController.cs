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
    public class PrivatePlayController : ControllerBase
    {
        private readonly RoomStorage _rooms;
        private readonly ILogger _logger;
        private readonly PlayRoomStorage _playRooms;
        private readonly PlaySessinRoomStorage _sessionPlayRooms;
        private readonly RoomStorage _session;
        private readonly LeaderboardStorage _leaderBoard; 
        private readonly JsonWorker<Leaderboard> _jsonUpdaterLeaderBoard;
        
        public PrivatePlayController(RoomStorage rooms, ILogger<PrivatePlayController> logger, PlayRoomStorage playRooms, PlaySessinRoomStorage sessionPlayRooms, RoomStorage session, LeaderboardStorage leaderBoard, JsonWorker<Leaderboard> jsonUpdaterLeaderBoard)
        {
            _rooms = rooms;
            _logger = logger;
            _playRooms = playRooms;
            _sessionPlayRooms = sessionPlayRooms;
            _session = session;
            _leaderBoard = leaderBoard;
            _jsonUpdaterLeaderBoard = jsonUpdaterLeaderBoard;
        }

        [HttpGet("create/{login}")]
        public ActionResult<Guid> StartGame(string login)
        {
            _logger.LogInformation($"{login} want to create a room ");
            var newRoom = new Room()
            {
                Guid = Guid.NewGuid(),
                Player1 = login,
                Player2 = null
            };

            _rooms.AddAsync(newRoom);
            _logger.LogInformation($"{login}  created  room ");
            return newRoom.Guid;
                

        }
        
        [HttpGet("{login}/{linkOfGuid}")]
        public async Task<IActionResult> WaitingLobby(string login,Guid linkOfGuid)
        {
            var waitingRoom = _rooms.SelectRoom(linkOfGuid);
            if (waitingRoom == Guid.Empty)
            {
                return NotFound();
            }

            var newElem = await _session.Get(linkOfGuid);
            if (newElem != null)
            {
                return Ok();
            }
            
            var newSessionRoom = await _rooms.Get(waitingRoom);
            if (newSessionRoom.Player1 == login)
            {
                return NotFound();
            }
            
            newSessionRoom.Player2 = login;
            await _session.AddWithGuidAsync(newSessionRoom.Guid, newSessionRoom);

            _logger.LogInformation($"{login} join into private game with {linkOfGuid}");
            return Ok();
            
            
        }
        [HttpPost("{linkOfGuid}")]
        public async Task<IActionResult> PlayGameAsync(Guid linkOfGuid, Player player)
        {
            _logger.LogInformation($"room with id {linkOfGuid} start there game");
            await _sessionPlayRooms.DeleteAsync(linkOfGuid);

            var room = await _playRooms.Get(linkOfGuid);

            if (room == null)
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
                return "Exit";
            }
            var game = new GameProcess(room.FirstPlayer, room.SecondPlayer);
            var winner = await game.PlayersPlay();
            if (winner.Value == "Exit")
            {
                _jsonUpdaterLeaderBoard.UpdateFile("Leaderboard.json", _leaderBoard.GetDictionary());
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


