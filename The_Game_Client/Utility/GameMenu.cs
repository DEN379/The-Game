using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Timers;
using The_Game_Client.Model;

namespace The_Game_Client.Utility
{
    class GameMenu
    {
        private Menu menu;
        private HttpClient client;
        private Auth auth;
        public static User User { get; private set; }
        private Timer timer = new Timer(5000);

        public GameMenu()
        {

            string logo = @" _____ _            _____                      
|_   _| |          |  __ \                     
  | | | |__   ___  | |  \/ __ _ _ __ ___   ___ 
  | | | '_ \ / _ \ | | __ / _` | '_ ` _ \ / _ \
  | | | | | |  __/ | |_\ \ (_| | | | | | |  __/
  \_/ |_| |_|\___|  \____/\__,_|_| |_| |_|\___|
                                               
                                               ";
            string[] options = new string[] { "Login", "Registration", "Exit" };
            menu = new Menu(logo, options);
            var json = File.ReadAllText("settings.json");
            var settings = JsonSerializer.Deserialize<Settings>(json);
            client = new HttpClient();
            client.BaseAddress = new Uri(settings.BaseAddress);
            auth = new Auth(client);
        }


        public async Task RunMainMenuAsync()
        {
            int selected = menu.Run();

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

        public async Task RunGameMenuAsync()
        {
            string logo = @" _____                                            _      
|  __ \                                          | |     
| |  \/ __ _ _ __ ___   ___   _ __ ___   ___   __| | ___ 
| | __ / _` | '_ ` _ \ / _ \ | '_ ` _ \ / _ \ / _` |/ _ \
| |_\ \ (_| | | | | | |  __/ | | | | | | (_) | (_| |  __/
 \____/\__,_|_| |_| |_|\___| |_| |_| |_|\___/ \__,_|\___|
                                                         
                                                         ";

            string[] options = new string[] { "Random game", "Private game", "Training with bot", "Back" };
            Menu gameMenu = new Menu(logo, options);
            int selected = gameMenu.Run();

            //Stopwatch stopWatch = new Stopwatch();
            //stopWatch.Start();

            switch (selected)
            {
                case 0:
                    Console.Clear();
                    await RandomGameAsync();
                    break;
                case 1:
                    Console.Clear();
                    await RunMainMenuAsync();
                    break;
                case 2:
                    Console.Clear();
                    await RunMainMenuAsync();
                    break;
                case 3:
                    Console.Clear();
                    await RunMainMenuAsync();
                    break;
            }


            //stopWatch.Stop();
            //TimeSpan ts = stopWatch.Elapsed;
            
            await RunGameMenuAsync();
        }


        public async Task LoginAsync(Auth auth)
        {
            if (auth.CountOfBadLogin == 3)
            {
                Console.WriteLine("You're been blocked for too many try to login");
                SetTimer();
                await ReturnToMainMenuAsync();
            }
            else StopTimer();
            Console.WriteLine("Login: ");
            string login = Console.ReadLine();

            Console.WriteLine("Password: ");
            string password = Console.ReadLine();

            User user = new User() { Login = login, Password = password };

            bool isSuccess = await auth.PostAsync("/login", user);

            if (isSuccess)
            {
                User = user;
                auth.User = user;
                await RunGameMenuAsync();
            }

        }

        public async Task RegisterAsync(Auth auth)
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

            await auth.PostAsync("/registration", user);

        }

        public async Task RandomGameAsync()
        {
            bool running = true;
            if(await auth.GetAsync($"/api/RandomPlay/create/{User.Login}"))
            {
                string logo = "Choose a figure => ";
                string[] options = new string[] { "Rock", "Scissors", "Paper", "Exit" };
                Menu randomGameMenu = new Menu(logo, options);

                Commands command = Commands.Exit;
                while (running)
                {
                    int result = randomGameMenu.Run();
                    Console.WriteLine(result);
                    switch (result)
                    {
                        case 0: command = Commands.Stone;
                            break;
                        case 1: command = Commands.Scissors;
                            break;
                        case 2: command = Commands.Paper;
                            break;
                        default: command = Commands.Exit;
                            break;
                    }
                    running = await auth.PostFigureAsync(command);
                }
            }
        }

        //public async Task TheGame()
        //{

            
        //}

        public async Task ReturnToMainMenuAsync()
        {
            Console.WriteLine("Press any key to go back to main menu");
            Console.ReadKey();
            await RunMainMenuAsync();
        }

        private void SetTimer()
        {
            timer.Elapsed += Unblock;
            timer.Enabled = true;
        }
        private void StopTimer()
        {
            if(timer.Enabled)
                timer.Enabled = false;
        }

        private void Unblock(Object source, ElapsedEventArgs e)
        {
            auth.CountOfBadLogin = 0;
        }
    }
}
