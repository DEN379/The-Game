namespace The_Game.Models
{
    public class Leaderboard
    {
        public string Login { get; set; }
        public int Wins { get; set; }
        public int Loses { get; set; }
        public int Draws { get; set; }
        public int Total { get; set; }
    }
}