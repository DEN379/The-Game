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
            var response = await PostRequestAsync<User>(user, request);

            if (response.StatusCode == HttpStatusCode.BadRequest)
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

        public async Task<bool> GetAsync(string controller, string request)
        {
            var response = await client.GetAsync(controller + request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var content = await response.Content.ReadAsStringAsync();
                var guid = content.Trim('"');
                Guid = guid;
                await FindRoomAsync(controller, guid);
                return true;
            }
            return false;
        }
        public async Task<bool> GetCreateAsync(string controller, string request)
        {
            var response = await client.GetAsync(controller + request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var content = await response.Content.ReadAsStringAsync();
                var guid = content.Trim('"');
                Guid = guid;
                Console.WriteLine("Private id for your parner => ");
                Console.WriteLine(guid);
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
                return true;
            }
            return false;
        }

        public async Task<bool> GetPrivateAsync(string controller, string request)
        {
            var response = await client.GetAsync(controller + request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return true;
            }
            return false;
        }

        public async Task FindRoomAsync(string controller, string guid)
        {
            while (true)
            {
                var responseToStart = await client.GetAsync($"{controller}/{guid}");
                
                if (responseToStart.StatusCode == HttpStatusCode.OK) { Console.WriteLine("Es"); break; }
                await Task.Delay(2000);
            }
        }

        public async Task<bool> PostFigureAsync(string firstRequest, string secondRequest, Commands commands)
        {

            var player = new Player()
            {
                Login = User.Login,
                Password = User.Password,
                Command = commands
            };
            var responseForGame = await PostRequestAsync<Player>(player, $"/{firstRequest}/{Guid}");

            Console.WriteLine(player.Command);
            Console.WriteLine(Guid);
            Console.WriteLine(responseForGame.StatusCode);

            if (commands == Commands.Exit) return false;
            while (true)
            {
                var response = await client.GetAsync($"/{firstRequest}/{secondRequest}/{Guid}");
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("rez " + result);
                    if (result.Equals("Draw"))
                    {
                        Console.WriteLine("Draw");
                    }
                    else if (result.Equals(User.Login))
                    {
                        Console.WriteLine("You won!");
                    }
                    else if (result.Equals("Exit")) return false;
                    else
                    {
                        Console.WriteLine("You loose :(");
                    }
                    Console.ReadKey();
                    return true;

                    //await PrintGameResultAsync(response);
                }
                else
                {
                    Console.WriteLine(response.StatusCode);
                }
            }
        }

        private async Task<HttpResponseMessage> PostRequestAsync<T>(object obj, string request)
        {
            T newObj = (T)obj;
            var content = new StringContent(JsonConvert.SerializeObject(newObj), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(request, content);

            return response;
        }

        private async Task<bool> PrintGameResultAsync(HttpResponseMessage response)
        {
            var result = await response.Content.ReadAsStringAsync();
            Console.WriteLine("rez " + result);
            if (result.Equals("Draw"))
            {
                Console.WriteLine("Draw");
            }
            else if (result.Equals(User.Login))
            {
                Console.WriteLine("You won!");
            }
            else if (result.Equals("Exit")) return false;
            else
            {
                Console.WriteLine("You loose :(");
            }
            Console.ReadKey();
            return true;
            //else if (responseToStart.StatusCode == HttpStatusCode.BadRequest) return false;
        }
    }
}
