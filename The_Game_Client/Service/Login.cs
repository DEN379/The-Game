using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using The_Game_Client.Model;
using The_Game_Client.Service;

namespace The_Game_Client.Utility
{
    class Login
    {
        private PlayerPersonalStat stat;
        Authentification auth;
        public Login(Authentification auth)
        {
            this.auth = auth;
        }
        public async Task<Authentification> LoginAsync()
        {
            if (auth.CountOfBadLogin == 3)
            {
                Console.WriteLine("You're been blocked for too many try to login");
                return null;
            }
            Console.WriteLine("Login: ");
            string login = Console.ReadLine();

            Console.WriteLine("Password: ");
            string password = Console.ReadLine();

            User user = new User() { Login = login, Password = password };

            bool isSuccess = await auth.AuthPostAsync("/login", user);

            if (isSuccess)
            {
                auth.AuthUser = user;
                PersonalStatistic personalStatistic = new PersonalStatistic(auth);
                var stats = await personalStatistic.GetStatsAsync(user);
                stat = JsonConvert.DeserializeObject<PlayerPersonalStat>(stats);
                if (stat == null) stat = new PlayerPersonalStat()
                {
                    Login = user.Login,
                    TimeInGame = "00:00:00",
                    ChangesWinrate = new Dictionary<DateTime, float>()
                };
                auth.Stat = stat;
                //Console.WriteLine("You successfuly loged in");
                return auth;
            }
            Console.WriteLine("\nYou enterd wrong login or password");
            Console.WriteLine("Press any button to go back to main menu");
            Console.ReadKey();
            return null;

        }

    }
}
