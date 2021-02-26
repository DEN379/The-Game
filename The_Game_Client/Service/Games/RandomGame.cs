using System;
using System.Net;
using System.Threading.Tasks;
using The_Game_Client.Utility;

namespace The_Game_Client.Service.Games
{
    class RandomGame
    {
        private readonly Authentification auth;
        private GameProcess gameProcess;
        public RandomGame(Authentification auth)
        {
            this.auth = auth;
            
        }
        public async Task RandomGameAsync(string controller, string secondary)
        {
            var guid = await GetAsync($"/{controller}", $"/create/{auth.AuthUser.Login}");
            if (guid != null)
            {
                auth.Guid = guid;
                gameProcess = new GameProcess(auth);
                TheGame game = new TheGame(gameProcess);
                await game.Play(controller, secondary);
            }
        }

        public async Task<string> GetAsync(string controller, string request)
        {
            var response = await auth.client.GetAsync(controller + request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var content = await response.Content.ReadAsStringAsync();
                var guid = content.Trim('"');
                var game = await FindRoomAsync(controller, guid);
                if (!game)
                {
                    return null;
                }
                
                return guid;
            }
            return null;
        }
        public async Task<bool> FindRoomAsync(string controller, string guid)
        {
            TimerClass timer = new TimerClass(25000);
            timer.SetTimer();
            timer.StartTimer();
            Console.Write("Waiting");
            while (true)
            {
                if (timer.inGoing == "Exit")
                {
                    Console.WriteLine();
                    Console.WriteLine("We cant find a room for you, sorry try latter )");
                    return false;
                }
                Console.Write(".");
                var responseToStart = await auth.client.GetAsync($"{controller}/{guid}");

                if (responseToStart.StatusCode == HttpStatusCode.OK) {  return true; }
                await Task.Delay(2000);
            }
            
        }
    }
}
