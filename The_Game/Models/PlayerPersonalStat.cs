using System;
using System.Collections.Generic;

namespace The_Game.Models
{
    public class PlayerPersonalStat
    {
        public string Login { get; set; }
        public int Wins { get; set; }
        public int Loses { get; set; }
        public int Draws { get; set; }

        public float WinRate
        {
            get
            {
                if ((Wins + Loses + Draws) == 0) return 0;
                return (float) Wins / (Wins + Loses + Draws) * 100;
            }
        }

        public int ScissorsCount { get; set; }
        public int RockCount { get; set; }
        public int PaperCount { get; set; }

        public Dictionary<DateTime, float> ChangesWinrate;
        public string TimeInGame { get; set; }

        public override string ToString()
        {
            return "\n\tWins\tLosses\tDraws\tTotal\tWin Rate\tRock" +
                   "\tScissors\tPaper\tTime in game\n" +
                   $"\t{Wins}\t{Loses}\t{Draws}\t{Wins + Loses + Draws}\t{WinRate}%\t\t{RockCount}" +
                   $"\t{ScissorsCount}\t{PaperCount}\t{TimeInGame}\n";

        }
    }
}
