using The_Game.Interfaces;

namespace The_Game.Classes
{
    public class User:IUser
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }
}