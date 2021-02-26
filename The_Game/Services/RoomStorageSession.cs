using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using The_Game.Models;
using The_Game.Services.Storage;

namespace The_Game.Services
{
    public class RoomStorageSession:RoomsStorage<Room>
    {
        public RoomStorageSession()
        {
            DataBase = new ConcurrentDictionary<Guid, Room>();
        }

        public KeyValuePair<Guid, Room> TakeLastRoom()
        {
            return DataBase.FirstOrDefault();
        }

        public Guid SelectRoom(Guid guid)
        {
            return DataBase.Select(x => x).FirstOrDefault(x => x.Value.Guid == guid).Key;
        }
    }
}