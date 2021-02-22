using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace The_Game.Services
{
    public class JsonWorker<T>
    {
        public  async Task<ConcurrentDictionary<int,T>> ReadList(string path)
        {
            try
            {
                var listOfObjects = JsonConvert.DeserializeObject<ConcurrentDictionary<int,T>>(await File.ReadAllTextAsync(path));
                return listOfObjects;
            }
            catch( Exception ex)
            {
                var obj = ex;
                return null;
            }

           
        }

        public  async void UpdateFile(string path, ConcurrentDictionary<int, T> listOfObjects)
        {
            var jsonList =  JsonConvert.SerializeObject(listOfObjects);
            await File.WriteAllTextAsync(path, jsonList);
        }
    }
}