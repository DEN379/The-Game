using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace The_Game_Client.Utility
{
    class MainMenu
    {
        private readonly Login login;
        private readonly Registration registration;
        private readonly LeaderBoard leaderBoard;
        private Menu menu;
        public MainMenu(Login login, Registration registration, LeaderBoard leaderBoard)
        {
            
            string logo = @" _____ _            _____                      
|_   _| |          |  __ \                     
  | | | |__   ___  | |  \/ __ _ _ __ ___   ___ 
  | | | '_ \ / _ \ | | __ / _` | '_ ` _ \ / _ \
  | | | | | |  __/ | |_\ \ (_| | | | | | |  __/
  \_/ |_| |_|\___|  \____/\__,_|_| |_| |_|\___|
                                               
                                               ";
            string[] options = new string[] { "Login", "Registration", "LeaderBoard", "Exit" };
            menu = new Menu(logo, options);
            this.login = login;
            this.registration = registration;
            this.leaderBoard = leaderBoard;
        }

        public async Task RunMainMenuAsync()
        {
            int selected = menu.Run();

            switch (selected)
            {
                case 0:
                    Console.Clear();
                    var auth = await login.LoginAsync();
                    if (auth != null)
                    {
                        GameMenuu gameMenuu = new GameMenuu(auth);
                        await gameMenuu.RunGameMenuAsync();
                    }
                    break;
                case 1:
                    Console.Clear();
                    await registration.RegisterAsync();
                    break;
                case 2:
                    Console.Clear();
                    await leaderBoard.LeaderBoardAsync();
                    Console.ReadKey();
                    break;
                case 3:
                    Environment.Exit(1);
                    break;

            }

            await RunMainMenuAsync();
        }
    }
}
