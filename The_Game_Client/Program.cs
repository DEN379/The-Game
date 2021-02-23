//using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using The_Game_Client.Model;
using The_Game_Client.Utility;

namespace The_Game_Client
{
    class Program
    {
        public static async Task RunMainMenuAsync()
        {
            string logo = @" _____ _            _____                      
|_   _| |          |  __ \                     
  | | | |__   ___  | |  \/ __ _ _ __ ___   ___ 
  | | | '_ \ / _ \ | | __ / _` | '_ ` _ \ / _ \
  | | | | | |  __/ | |_\ \ (_| | | | | | |  __/
  \_/ |_| |_|\___|  \____/\__,_|_| |_| |_|\___|
                                               
                                               ";
            string[] options = new string[] { "Login", "Registration", "Exit" };
            Menu menu = new Menu(logo, options);
            int selected = menu.Run();

            var json = File.ReadAllText("settings.json");
            var settings = JsonSerializer.Deserialize<Settings>(json);
            var client = new HttpClient();
            client.BaseAddress = new Uri(settings.BaseAddress);
            Auth auth = new Auth(client);

            switch (selected)
            {
                case 0:
                    Console.Clear();
                    await LoginAsync(auth);
                    break;
                case 1:
                    Console.Clear();
                    await RegisterAsync(auth);
                    break;
                case 2:
                    Environment.Exit(1);
                    break;
            }



            await RunMainMenuAsync();
        }
        static async Task Main(string[] args)
        {
            await RunMainMenuAsync();
        }

        public static async Task LoginAsync(Auth auth)
        {
            Console.WriteLine("Login: ");
            string login = Console.ReadLine();

            Console.WriteLine("Password: ");
            string password = Console.ReadLine();

            User user = new User() { Login = login, Password = password };

            await auth.PostAsync("/login", user);

        }

        public static async Task RegisterAsync(Auth auth)
        {
            Console.WriteLine("Login: ");
            string login = Console.ReadLine();

            Console.WriteLine("Password: ");
            string password = Console.ReadLine();

            if (login.Length < 4)
            {
                Console.WriteLine("\nLogin length mast be more than 4!");
                await ReturnToMainMenuAsync();
            }
            if (password.Length < 6)
            {
                Console.WriteLine("\nPassword length must be 6 or more!");
                await ReturnToMainMenuAsync();
            }

            User user = new User() { Login = login, Password = password };

            await auth.PostAsync("/register", user);

        }

        public static async Task ReturnToMainMenuAsync()
        {
            Console.WriteLine("Press any key to go back to main menu");
            Console.ReadKey();
            await RunMainMenuAsync();
        }

    }
}
