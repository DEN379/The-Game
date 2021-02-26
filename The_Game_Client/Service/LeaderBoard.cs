using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace The_Game_Client.Utility
{
    class LeaderBoard
    {
        private readonly HttpClient client;

        public LeaderBoard(HttpClient client)
        {
            this.client = client;
        }
        public async Task LeaderBoardAsync()
        {
            var response = await client.GetAsync("/api/LeaderBoard");
            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine(content);
        }
    }
}
