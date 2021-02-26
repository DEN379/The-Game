using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Timers;
using The_Game_Client.Model;

namespace The_Game_Client.Utility
{
    class GameMenu
    {
        private HttpClient client;
        //private Auth auth;
        public static User User { get; private set; }
        private Timer timer = new Timer(5000);
        private PlayerPersonalStat stat;

        

        public async Task RunAsync()
        {
            var json = File.ReadAllText("settings.json");
            var settings = JsonConvert.DeserializeObject<Settings>(json);
            client = new HttpClient();
            client.BaseAddress = new Uri(settings.BaseAddress);
            Authentification auth = new Authentification(client);
            Login login = new Login(auth);
            Registration registration = new Registration(auth);
            LeaderBoard leaderBoard = new LeaderBoard(client);
            MainMenu mainMenu = new MainMenu(login, registration,leaderBoard);
            await mainMenu.RunMainMenuAsync();

        }




    

        //public async Task ReturnToMainMenuAsync()
        //{
        //    Console.WriteLine("Press any key to go back to main menu");
        //    Console.ReadKey();
        //    await RunMainMenuAsync();
        //}

        //private void SetTimer()
        //{
        //    timer.Elapsed += Unblock;
        //    timer.Enabled = true;
        //}
        //private void StopTimer()
        //{
        //    if(timer.Enabled)
        //        timer.Enabled = false;
        //}

        //private void Unblock(Object source, ElapsedEventArgs e)
        //{
        //    auth.CountOfBadLogin = 0;
        //}
    }
}
