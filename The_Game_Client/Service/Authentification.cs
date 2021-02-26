using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using The_Game_Client.Model;

namespace The_Game_Client.Utility
{
    class Authentification
    {
        public User AuthUser { get; set; }

        public string Guid { get; set; }
        public HttpClient client { get; set; }

        public int CountOfBadLogin { get; set; } = 0;

        public Authentification(HttpClient client)
        {
            this.client = client;
            
        }

        public async Task<bool> AuthPostAsync(string request, User user)
        {
            PostRequest postRequest = new PostRequest(client);
            var response = await postRequest.ExecuteAsync<User>(user, request);

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

     
    }
}
