﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace The_Game_Client.Utility
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
                //Guid = guid;
                await FindRoomAsync(controller, guid);
                return guid;
            }
            return null;
        }
        public async Task FindRoomAsync(string controller, string guid)
        {
            while (true)
            {
                var responseToStart = await auth.client.GetAsync($"{controller}/{guid}");

                if (responseToStart.StatusCode == HttpStatusCode.OK) { Console.WriteLine("Es"); break; }
                await Task.Delay(2000);
            }
        }
    }
}
