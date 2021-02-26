using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using The_Game.Models;
using The_Game_Client.Model;

namespace The_Game_Client.Utility
{
    class GameProcess
    {
        private readonly Authentification auth;

        public GameProcess(Authentification auth)
        {
            this.auth = auth;
        }
        public async Task<bool> PostFigureAsync(string firstRequest, string secondRequest, Commands commands, TimerClass timer)
        {
           
            PostRequest postRequest = new PostRequest(auth.client);
            var player = new Player()
            {
                Login = auth.AuthUser.Login,
                Password = auth.AuthUser.Password,
                Command = commands
            };
            Console.WriteLine($"/{firstRequest}/{auth.Guid}");
            var responseForGame = await postRequest.ExecuteAsync<Player>(player, $"/{firstRequest}/{auth.Guid}");

            Console.WriteLine(player.Command);
            Console.WriteLine(auth.Guid);
            Console.WriteLine(responseForGame.StatusCode);

            if (commands == Commands.Exit)
            {
                await auth.client.GetAsync($"/{firstRequest}/{secondRequest}/{auth.Guid}");
                return false;
            }

            while (true)
            {
                if (timer.inGoing == "Exit")
                {
                    await auth.client.GetAsync($"/{firstRequest}/{secondRequest}/{auth.Guid}");
                    return false;
                }
                Console.WriteLine($"/{firstRequest}/{secondRequest}/{auth.Guid}");
                var response = await auth.client.GetAsync($"/{firstRequest}/{secondRequest}/{auth.Guid}");
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("rez " + result);
                    if (result.Equals("Draw"))
                    {
                        Console.WriteLine("Draw");
                    }
                    else if (result.Equals(auth.AuthUser.Login))
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
    }
}
