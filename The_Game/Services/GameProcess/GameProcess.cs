using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using The_Game.Models;

namespace The_Game.Services.GameProcess
{
    public class GameProcess : IGameProcess
    {
        private readonly Player _player1;
        private readonly Player _player2;

        public GameProcess(Player player1,Player player2)
        {
            _player1 = player1;
            _player2 = player2;
        }
        
        public  async Task<ActionResult<string>> PlayersPlay()
        {
            if (_player1.Command == Commands.Exit || _player2.Command == Commands.Exit)
            {
                return "Exit";
            }
            if (_player1.Command == _player2.Command)
            {
                return "Draw";
            } 
            
            var winner = await WinCondition(_player1.Command, _player2.Command);
            return _player1.Command == winner ? _player1.Login : _player2.Login;
        }

        public async Task<Commands?> WinCondition(Commands command, Commands command2)
        {
            if (command == command2)
            {
                return command;
            }

            switch (command)
            {
                case Commands.Scissors:
                    switch (command2)
                    {
                        case Commands.Rock:
                            return Commands.Rock;
                        case Commands.Paper:
                            return Commands.Scissors;
                    }

                    break;
                case Commands.Rock:
                    switch (command2)
                    {
                        case Commands.Scissors:
                            return Commands.Rock;
                        case Commands.Paper:
                            return Commands.Paper;
                    }

                    break;
                case Commands.Paper:
                    switch (command2)
                    {
                        case Commands.Rock:
                            return Commands.Paper;
                        case Commands.Scissors:
                            return Commands.Scissors;
                    }

                    break;
            }

            return null;


        }


    }
}