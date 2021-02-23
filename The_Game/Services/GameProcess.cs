using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using The_Game.Models;

namespace The_Game.Services
{
    public class GameProcess
    {
        public  async Task<string> PlayersPlay(Player player1, Player player2 )
        {
            if (player1.Figure == player2.Figure)
            {
                return "Draw";
            } 
            
            var winner = await WinCondition(player1.Figure, player2.Figure);
            return player1.Figure == winner ? player1.Login : player2.Login;
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