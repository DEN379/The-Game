using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using The_Game.Classes;

namespace The_Game.Models
{
    public class Room
    {
        public Guid Guid { get; set; }
        public string Player1 { get; set; }
        public string Player2 { get; set; }

    }
}
