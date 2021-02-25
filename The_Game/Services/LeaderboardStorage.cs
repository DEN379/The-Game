using System.Collections.Concurrent;
using System.Linq;
using The_Game.Classes;
using The_Game.Models;

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


    }
}