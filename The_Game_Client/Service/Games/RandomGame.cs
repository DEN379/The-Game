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
                await FindRoomAsync(controller, guid);
                return guid;
            }
            return null;
        }
        public async Task FindRoomAsync(string controller, string guid)
        {
            Console.Write("Waiting");
            while (true)
            {
                Console.Write(".");
                var responseToStart = await auth.client.GetAsync($"{controller}/{guid}");

                if (responseToStart.StatusCode == HttpStatusCode.OK) {  break; }
                await Task.Delay(2000);
            }
        }
    }
}
