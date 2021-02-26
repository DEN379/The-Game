using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using The_Game_Client.Model;

namespace The_Game_Client.Utility
{
    class PrivateGame
    {
        private readonly Authentification auth;

        public PrivateGame(Authentification authentification)
        {
            this.auth = authentification;
        }
        public async Task PrivateGameAsync(string controller, string secondary)
        {

            string logo = "Create a new game or connect to exist server => ";
            string[] options = new string[] { "Join", "Create", "Exit" };
            Menu privateGameMenu = new Menu(logo, options);

            int result = privateGameMenu.Run();
            bool isAuth = false;
            switch (result)
            {
                case 0:
                    Console.WriteLine("Enter pls your partner's id to enter to lobby: ");
                    string guid = Console.ReadLine().Trim();

                    if (isAuth = await GetPrivateAsync($"/{controller}", $"/{auth.AuthUser.Login}/{guid}"))
                    {
                        auth.Guid = guid;
                    }

                    break;
                case 1:
                    isAuth = await GetCreateAsync($"/{controller}", $"/create/", $"{auth.AuthUser.Login}");
                    break;
                default: return;
            }
            if (isAuth)
            {
                GameProcess gameProcess = new GameProcess(auth);
                TheGame game = new TheGame(gameProcess);
                await game.Play(controller, secondary);
            }

        }



        public async Task<bool> GetCreateAsync(string controller, string secondary, string login)
        {
            var response = await auth.client.GetAsync(controller + secondary + login);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var content = await response.Content.ReadAsStringAsync();
                var guid = content.Trim('"');
                auth.Guid = guid;
                Console.WriteLine("Private id for your parner => ");
                Console.WriteLine(guid);
                Console.WriteLine("\nPress any key to continue...");
                TimerClass timer = new TimerClass(25000);
                timer.SetTimer();
                timer.StartTimer();
                while (true)
                {
                    response = await auth.client.GetAsync($"{controller}/{login}/{guid}");

                    if (response.StatusCode == HttpStatusCode.OK) { break; }
                    await Task.Delay(2000);
                }

                return true;
            }
            return false;
        }

        public async Task<bool> GetPrivateAsync(string controller, string request)
        {
            
            while (true)
            {
                
                var responseToStart = await auth.client.GetAsync(controller + request);

                if (responseToStart.StatusCode == HttpStatusCode.OK) { return true; }
                await Task.Delay(2000);
            }

            
        }
    }
}
