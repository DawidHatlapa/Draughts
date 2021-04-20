using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace polish_draughts_csharp_Boner1975
{
    public class Game
    {

        public Player firstPlayer;
        public Player secondPlayer;
        private Board currentBoard;

        public Game(Player firstPlayer, Player secondPlayer, Board currentBoard)
        {
            this.firstPlayer = firstPlayer;
            this.secondPlayer = secondPlayer;
            this.currentBoard = currentBoard;
        }

        public void Playing()
        {
            bool isWinner = false;
            while (!isWinner)
            {
                this.currentBoard = firstPlayer.Move(this.currentBoard, this);
                if (checkWinner(currentBoard, firstPlayer, secondPlayer)) break;

                this.currentBoard = secondPlayer.Move(this.currentBoard, this);
                if (checkWinner(currentBoard, firstPlayer, secondPlayer)) break;
            }
        }

        private bool checkWinner(Board board, Player firstPlayer, Player secondPlayer)
        {
            bool isWinner = false;
            if (firstPlayer.itemNumber == 0)
            {
                Comunicator.PrintResult(secondPlayer);
                isWinner = true;
            }
            else if (secondPlayer.itemNumber == 0)
            {
                Comunicator.PrintResult(firstPlayer);
                isWinner = true;
            }
            return isWinner;
        }
    }
}
