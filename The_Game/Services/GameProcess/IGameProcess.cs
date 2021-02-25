using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using The_Game.Models;

namespace The_Game.Services
{
    public interface IGameProcess
    {
        public  Task<ActionResult<string>> PlayersPlay();
        public Task<Commands?> WinCondition(Commands command, Commands command2);
    }
}