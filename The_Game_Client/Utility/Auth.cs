using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using The_Game.Models;
using The_Game_Client.Model;

namespace The_Game_Client.Utility
{
    class Auth
    {
        public int CountOfBadLogin { get; set; } = 0;
        public string Guid { get; set; }

        public User User { get; set; }

        private HttpClient client;

        public Auth(HttpClient client)
        {
            this.client = client;
        }
        

        public async Task<bool> PostAsync(string request, User user)
        {
            var userObj = JsonConvert.SerializeObject(user);
            var content = new StringContent(userObj, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(request, content);

            if(response.StatusCode == HttpStatusCode.BadRequest)
            {
                CountOfBadLogin++;
            }
            else if (response.StatusCode == HttpStatusCode.OK)
            {
                CountOfBadLogin = 0;
                return true;
            }
            return false;
        }

        public async Task<bool> GetAsync(string request)
        {
            var response = await client.GetAsync(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var content = await response.Content.ReadAsStringAsync();
                var guid = content.Trim('"');
                Guid = guid;
                await FindRoomAsync(guid);
                return true;
            }
            return false;
        }

        public async Task FindRoomAsync(string guid)
        {
            while (true)
            {
                var responseToStart = await client.GetAsync($"/api/RandomPlay/{guid}");
                
                if (responseToStart.StatusCode == HttpStatusCode.OK) { Console.WriteLine("Es"); break; }
                await Task.Delay(2000);
            }
        }

        public async Task<bool> PostFigureAsync(Commands commands)
        {

            var player = new Player()
            {
                Login = User.Login,
                Password = User.Password,
                Command = commands
            };
            var content = new StringContent(JsonConvert.SerializeObject(player), Encoding.UTF8, "application/json");
            var responseForGame = await client.PostAsync($"/api/RandomPlay/{Guid}", content);
            Console.WriteLine(player.Command);
            Console.WriteLine(Guid);
            Console.WriteLine(responseForGame.StatusCode);
            while (true)
            {
                var responseToStart = await client.GetAsync($"/api/RandomPlay/game/{Guid}");
                if (responseToStart.StatusCode == HttpStatusCode.OK)
                {
                    var result = await responseToStart.Content.ReadAsStringAsync();
                    Console.WriteLine("rez "+result);
                    if (result.Equals("Draw"))
                    {
                        Console.WriteLine("Draw");
                        Console.ReadKey();
                        return true;
                    }
                    else if (result.Equals("Exit")) return false;
                    else if (result.Equals(User.Login))
                    {
                        Console.WriteLine("You won!");
                        Console.ReadKey();
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("You loose :(");
                        Console.ReadKey();
                        return true;
                    }
                }
                else
                {
                    Console.WriteLine(responseToStart.StatusCode);
                }

            }
        }
    }
}
