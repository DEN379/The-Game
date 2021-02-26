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
        
        public string Guid { get; set; }

        public User User { get; set; }

        private HttpClient client;

        public Auth(HttpClient client)
        {
            this.client = client;
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
