using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace The_Game.Classes
{
    public class JsonWorker<T>
    {
        public  async Task<List<T>> ReadList(string path)
        {
            var listOfObjects =  JsonSerializer.Deserialize<List<T>>(await File.ReadAllTextAsync(path));
            return listOfObjects;
        }

        public  async void UpdateFile(string path, List<T> listOfObjects)
        {
            var jsonList =  JsonSerializer.Serialize(listOfObjects);
            await File.WriteAllTextAsync(path, jsonList);

        }
    }
}