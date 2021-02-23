using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using The_Game_Client.Model;

namespace The_Game_Client.Utility
{
    class Auth
    {
        private HttpClient client;

        public Auth(HttpClient client)
        {
            this.client = client;
        }
        

        public async Task PostAsync(string request, User user)
        {
            var userObj = JsonConvert.SerializeObject(user);
            var content = new StringContent(userObj, Encoding.UTF8, "application/json");
            await client.PostAsync(request, content);
        }
    }
}
