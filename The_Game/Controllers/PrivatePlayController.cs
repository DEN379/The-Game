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
           
            return Ok();
            
            
        }

        [HttpPost("{linkOfGuid}")]
        public async Task<IActionResult> PlayGameAsync(Guid linkOfGuid, Player player)
        {
            
            await _sessionPlayRooms.DeleteAsync(linkOfGuid);
            
            var room = await _playRooms.Get(linkOfGuid);
            if (room == null)
            {
                var newPlayRoom = new PlayRoom()
                {
                    FirstPlayer = player,
                    SecondPlayer = null
                };
                
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
            var game = new GameProcess(room.FirstPlayer, room.SecondPlayer);
            var winner = await game.PlayersPlay();
            if (winner.Value == "Exit")
            {
                _jsonUpdaterLeaderBoard.UpdateFile("Leaderboard.json", _leaderBoard.GetDictionary());
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


