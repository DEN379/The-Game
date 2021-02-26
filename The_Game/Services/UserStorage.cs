using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using The_Game.Classes;
using The_Game.Services.Storage;

namespace The_Game.Services
{
    public class UserStorage:Storage<User>
    {
        private readonly JsonWorker<User> _jsonReader = new JsonWorker<User>();
        public UserStorage()
        {

            DataBase = _jsonReader.ReadList("Users.json").Result ?? new ConcurrentDictionary<int, User>();
        }

        public bool FindUser(User user)
        {
            var find = DataBase.Select(x => x.Value).FirstOrDefault(x => x.Login == user.Login);
            return find != null && find.Password == user.Password;
        }

        



    }
}