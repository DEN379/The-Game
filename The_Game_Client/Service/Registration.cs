using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using The_Game_Client.Model;

namespace The_Game_Client.Utility
{
    class Registration
    {
        Authentification auth;
        public Registration(Authentification auth)
        {
            this.auth = auth;
        }
        public async Task<bool> RegisterAsync()
        {
            Console.WriteLine("Login: ");
            string login = Console.ReadLine();

            Console.WriteLine("Password: ");
            string password = Console.ReadLine();

            if (login.Length < 4)
            {
                Console.WriteLine("\nLogin length mast be more than 4!");
                //await ReturnToMainMenuAsync();
                return false;
            }
            if (password.Length < 6)
            {
                Console.WriteLine("\nPassword length must be 6 or more!");
                //await ReturnToMainMenuAsync();
                return false;
            }

            User user = new User() { Login = login, Password = password };

            await auth.AuthPostAsync("/registration", user);
            return true;

        }
    }
}
