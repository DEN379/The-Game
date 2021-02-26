using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using The_Game_Client.Model;
using The_Game_Client.Utility;

namespace The_Game_Client.Service
{
    class PersonalStatistic
    {
        private readonly Authentification auth;

        public PersonalStatistic(Authentification auth)
        {
            this.auth = auth;
        }

        public async Task<string> GetStatsAsync(User user)
        {
            var response = await auth.client.GetAsync($"/api/PersonalPlayersStat/{user.Login}");
            return await response.Content.ReadAsStringAsync();
        }

        public async Task PostStatsAsync(PlayerPersonalStat stat)
        {

            var content = new StringContent(JsonConvert.SerializeObject(stat), Encoding.UTF8, "application/json");
            var response = await auth.client.PostAsync($"/api/PersonalPlayersStat", content);
        }
    }
}
