using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using The_Game.Models;
using The_Game.Services.Storage;

namespace The_Game.Services
{
    public class StorageOfPersonalStat :Storage<PlayerPersonalStat>
    {
        private readonly JsonWorker<PlayerPersonalStat> _playerStatWorker = new JsonWorker<PlayerPersonalStat>();
        public StorageOfPersonalStat()
        {
            DataBase = _playerStatWorker.ReadList("PlayerPersonalStat.json").Result ?? new ConcurrentDictionary<int, PlayerPersonalStat>();
        }

        public void UpdateStat(PlayerPersonalStat stat)
        {
            var newStat = DataBase.FirstOrDefault(x => x.Value.Login == stat.Login);
            if (newStat.Value == null)
            {
                AddAsync(stat);
                return;
            }

            AddOrUpdateAsync(newStat.Key, newStat.Value);

        }

        public Task UpdateStatAsync(PlayerPersonalStat stat)
        {
            UpdateStat(stat);
            return Task.CompletedTask;

        }
    }
}
