//using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using The_Game_Client.Model;
using The_Game_Client.Utility;

namespace The_Game_Client
{
    class Program
    {
        

        static async Task Main(string[] args)
        {
            GameMenu gameMenu = new GameMenu();
            await gameMenu.RunMainMenuAsync();
        }

    }
}
