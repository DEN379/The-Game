using System.Collections.Concurrent;
using System.Linq;
using The_Game.Classes;

namespace The_Game.Services
{
    public class UserStorage:Storage<User>
    {
        private readonly JsonWorker<User> _jsonReader = new JsonWorker<User>();
        public UserStorage()
        {

            _dbPlayRooms = _jsonReader.ReadList("Users.json").Result ?? new ConcurrentDictionary<int, User>();
        }

        public bool FindUser(User user)
        {
            var find = _dbPlayRooms.Select(x => x.Value).FirstOrDefault(x => x.Login == user.Login);
            return find != null;
        }

    }
}