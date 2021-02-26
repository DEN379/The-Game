using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using The_Game.Models;
using The_Game_Client.Model;

namespace The_Game_Client.Utility
{
    class GameWithBot
    {
        private readonly Authentification auth;

        public GameWithBot(Authentification auth)
        {
            this.auth = auth;
        }
        public async Task BotGame(string controller)
        {
            string logo = "Choose a figure => ";
            string[] options = new string[] { "Rock", "Scissors", "Paper", "Exit" };
            Menu randomGameMenu = new Menu(logo, options);

            Commands command = Commands.Exit;
            bool isRunning = true;
            while (isRunning)
            {
                int result = randomGameMenu.Run();
                Console.WriteLine(result);
                switch (result)
                {
                    case 0:
                        command = Commands.Stone;
                        break;
                    case 1:
                        command = Commands.Scissors;
                        break;
                    case 2:
                        command = Commands.Paper;
                        break;
                    default:
                        command = Commands.Exit;
                        break;
                }
                isRunning = await PostFigureBotAsync(controller, command);
            }
        }


        public async Task<bool> PostFigureBotAsync(string firstRequest, Commands commands)
        {
            PostRequest postRequest = new PostRequest(auth.client);
            var player = new Player()
            {
                Login = auth.AuthUser.Login,
                Password = auth.AuthUser.Password,
                Command = commands
            };
            var response = await postRequest.ExecuteAsync<Player>(player, $"/{firstRequest}");
            return await PrintGameResultAsync(response);
        }

        private async Task<bool> PrintGameResultAsync(HttpResponseMessage response)
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
            //else if (responseToStart.StatusCode == HttpStatusCode.BadRequest) return false;
        }
    }
}
