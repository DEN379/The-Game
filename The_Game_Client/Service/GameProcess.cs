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
        public Authentification auth;

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
            var responseForGame = await postRequest.ExecuteAsync<Player>(player, $"/{firstRequest}/{auth.Guid}");


            if (commands == Commands.Exit)
            {
                await auth.client.GetAsync($"/{firstRequest}/{secondRequest}/{auth.Guid}");
                return false;
            }

            Console.Write("\nWaiting for the oponent\n");
            while (true)
            {
                
                if (timer.inGoing == "Exit")
                {
                    await auth.client.GetAsync($"/{firstRequest}/{secondRequest}/{auth.Guid}");
                    return false;
                }
                var response = await auth.client.GetAsync($"/{firstRequest}/{secondRequest}/{auth.Guid}");
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    if (result.Equals("Draw"))
                    {
                        auth.Stat.Draws++;
                        Console.WriteLine("Draw");
                        auth.Stat.Draws++;
                    }
                    else if (result.Equals(auth.AuthUser.Login))
                    {
                        auth.Stat.Wins++;
                        Console.WriteLine("You won!");
                        auth.Stat.Wins++;
                    }
                    else if (result.Equals("Exit")) return false;
                    else
                    {
                        auth.Stat.Loses++;
                        Console.WriteLine("You loose :(");
                        auth.Stat.Loses++;
                    }
                    Console.ReadKey();
                    timer.TimerDispose();
                    return true;
                }
                else
                {
                    //Console.WriteLine(response.StatusCode);
                    //Console.Write(".");
                }
            }
        }
    }
}
