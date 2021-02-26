﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using The_Game_Client.Model;

namespace The_Game_Client.Utility
{
    class TheGame
    {
        private readonly GameProcess gameProcess;
        
        public TheGame(GameProcess gameProcess)
        {
            this.gameProcess = gameProcess;
        }
        public async Task Play(string controller, string secondary)
        {
            var timer = new TimerClass(20000);
            timer.SetTimer();
            timer.StartTimer();

            string logo = "Choose a figure => ";
            string[] options = new string[] { "Rock", "Scissors", "Paper", "Exit" };
            Menu randomGameMenu = new Menu(logo, options);

            Commands command = Commands.Exit;
            bool isRunning = true;
            while (isRunning)
            {

                if (timer.inGoing == "Exit")
                {
                    command = Commands.Exit;
                    isRunning = await gameProcess.PostFigureAsync(controller, secondary, command,timer);
                    return;
                }
                int result = randomGameMenu.Run();
                Console.WriteLine(result);

                switch (result)
                {
                    case 0:
                        command = Commands.Stone;
                        break;
                    case 1:
                        command = Commands.Scissors;
                        break;
                    case 2:
                        command = Commands.Paper;
                        break;
                    default:
                        command = Commands.Exit;
                        break;
                }
                isRunning = await gameProcess.PostFigureAsync(controller, secondary, command,timer);

            }
        }
    }
}
