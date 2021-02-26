using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace The_Game_Client.Utility
{
    class PostRequest
    {
        private readonly HttpClient client;

        public PostRequest(HttpClient client)
        {
            this.client = client;
        }
        public async Task<HttpResponseMessage> ExecuteAsync<T>(object obj, string request)
        {
            T newObj = (T)obj;
            var content = new StringContent(JsonConvert.SerializeObject(newObj), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(request, content);

            return response;
        }
    }
}
