using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace The_Game.Models
{
    public class PlayerPersonalStat
    {
        public string Login { get; set; }
        public int Wins { get; set; }
        public int Loses { get; set; }
        public int Draws { get; set; } 
        public float WinRate { get; set; }
        public int ScissorsCount { get; set; }
        public int RockCount { get; set; }
        public int PaperCount { get; set; }

        public Dictionary<DateTime, float> ChangesWinrate;
        // if currentDateTime = ChangesWinragte.key {value = }
        public string TimeInGame { get; set; }

    }
}
