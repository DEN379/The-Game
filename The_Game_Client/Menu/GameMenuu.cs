using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using The_Game_Client.Service;
using The_Game_Client.Service.Games;

namespace The_Game_Client.Utility
{
    class GameMenuu
    {
        private readonly Authentification auth;

        public GameMenuu(Authentification auth)
        {
            this.auth = auth;
        }
        public async Task RunGameMenuAsync()
        {
            string logo = @" _____                                            _      
|  __ \                                          | |     
| |  \/ __ _ _ __ ___   ___   _ __ ___   ___   __| | ___ 
| | __ / _` | '_ ` _ \ / _ \ | '_ ` _ \ / _ \ / _` |/ _ \
| |_\ \ (_| | | | | | |  __/ | | | | | | (_) | (_| |  __/
 \____/\__,_|_| |_| |_|\___| |_| |_| |_|\___/ \__,_|\___|
                                                         
                                                         ";

            string[] options = new string[] { "Random game", "Private game", "Training with bot", "Back" };
            Menu gameMenu = new Menu(logo, options);
            int selected = gameMenu.Run();

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            PersonalStatistic ps = new PersonalStatistic(auth);

            switch (selected)
            {
                case 0:
                    Console.Clear();
                    RandomGame randomGame = new RandomGame(auth);
                    await randomGame.RandomGameAsync("api/RandomPlay", "game");
                    break;
                case 1:
                    Console.Clear();
                    PrivateGame privateGame = new PrivateGame(auth);
                    await privateGame.PrivateGameAsync("api/PrivatePlay", "game");
                    break;
                case 2:
                    Console.Clear();
                    GameWithBot gameWithBot = new GameWithBot(auth);
                    await gameWithBot.BotGame("api/BotPlay");
                    break;
                case 3:
                    Console.Clear();
                    stopWatch.Stop();
                    TimeSpan timeSpan = stopWatch.Elapsed;
                    TimeSpan ts = TimeSpan.Parse(auth.Stat.TimeInGame);
                    auth.Stat.TimeInGame = ts.Add(timeSpan).ToString();
                    await ps.PostStatsAsync(auth.Stat);
                    return;
                    //await RunGameMenuAsync();
                    //await RunMainMenuAsync();
                    break;
            }


            //stopWatch.Stop();
            //TimeSpan ts = stopWatch.Elapsed;
            //stat.TimeInGame.Add(ts);
            //await PostStatsAsync(stat);

            await RunGameMenuAsync();
        }
    }
}
