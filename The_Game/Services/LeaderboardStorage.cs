using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using The_Game.Classes;
using The_Game.Models;
using The_Game.Services.Storage;

namespace The_Game.Services
{
    public class LeaderboardStorage : Storage<Leaderboard>
    {
        private readonly JsonWorker<Leaderboard> _jsonReader = new JsonWorker<Leaderboard>();
        public LeaderboardStorage()
        {

            DataBase = _jsonReader.ReadList("Leaderboard.json").Result ?? new ConcurrentDictionary<int, Leaderboard>();
        }

        public bool FindUser(User user)
        {
            var find = DataBase.Select(x => x.Value).FirstOrDefault(x => x.Login == user.Login);
            return find != null;
        }

        public Task AddWins(string login)
        {
            var lead = DataBase.FirstOrDefault(x => x.Value.Login == login).Value;
            lead.Wins++;
            lead.Total++;
            return Task.CompletedTask;
            
        }
        public Task AddLoses(string login)
        {
            var lead = DataBase.FirstOrDefault(x => x.Value.Login == login).Value;
            lead.Loses++;
            lead.Total++;
            return Task.CompletedTask;

        }
        public Task AddDraws(string login)
        {
            var lead = DataBase.FirstOrDefault(x => x.Value.Login == login).Value;
            lead.Draws++;
            lead.Total++;
            return Task.CompletedTask;

        }


    }
}