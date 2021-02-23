using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using The_Game.Models;

namespace The_Game.Services
{
    public class GameProcess
    {
        private readonly Player _player1;
        private readonly Player _player2;

        public GameProcess(Player player1,Player player2)
        {
            _player1 = player1;
            _player2 = player2;
        }
        
        public  async Task<string> PlayersPlay()
        {
            if (_player1.Figure == _player2.Figure)
            {
                return "Draw";
            } 
            
            var winner = await WinCondition(_player1.Figure, _player2.Figure);
            return _player1.Figure == winner ? _player1.Login : _player2.Login;
        }

        private async Task<Figures?> WinCondition(Figures figure, Figures figure2)
        {
            if (figure == figure2)
            {
                return figure;
            }

            switch (figure)
            {
                case Figures.Scissors:
                    switch (figure2)
                    {
                        case Figures.Stone:
                            return Figures.Stone;
                        case Figures.Paper:
                            return Figures.Scissors;
                    }

                    break;
                case Figures.Stone:
                    switch (figure2)
                    {
                        case Figures.Scissors:
                            return Figures.Stone;
                        case Figures.Paper:
                            return Figures.Paper;
                    }

                    break;
                case Figures.Paper:
                    switch (figure2)
                    {
                        case Figures.Stone:
                            return Figures.Paper;
                        case Figures.Scissors:
                            return Figures.Scissors;
                    }

                    break;
                
                   
            }

            return null;


        }


    }
}