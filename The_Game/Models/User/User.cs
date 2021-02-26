namespace The_Game.Models.User
{
    public class User:IUser
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }
}