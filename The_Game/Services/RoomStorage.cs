using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using The_Game.Classes;
using The_Game.Models;

namespace The_Game.Services
{
    public class RoomStorage : Storage<Room>
    {
        public RoomStorage()
        {
            DataBase = new ConcurrentDictionary<int, Room>();
        }

        public KeyValuePair<int, Room> TakeLastRoom()
        {
            return DataBase.FirstOrDefault();
        }

        public int SelectRoom(Guid guid)
        {
            return DataBase.Select(x => x).FirstOrDefault(x => x.Value.Guid == guid).Key;
        }
    }

    
}
