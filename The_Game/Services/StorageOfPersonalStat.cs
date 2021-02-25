using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using The_Game.Models;

namespace The_Game.Services
{
    public class StorageOfPersonalStat :Storage<PlayerPersonalStat>
    {
        private readonly JsonWorker<PlayerPersonalStat> _playerStatWorker = new JsonWorker<PlayerPersonalStat>();
        public StorageOfPersonalStat()
        {
            DataBase = _playerStatWorker.ReadList("PlayerPersonalStat.json").Result ?? new ConcurrentDictionary<int, PlayerPersonalStat>();
        }
    }
}
