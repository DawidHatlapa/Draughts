using System;


namespace polish_draughts_csharp_Boner1975
{
    public class Board
    {
        public int boardSize;
        public string[,] pawn;
        public int col;
        public int row;

        public Board(Player firstPlayer, Player seccondPlayer)
        {
            this.boardSize = GetBoardSize();
            this.pawn = FillBoard(firstPlayer, seccondPlayer, this.boardSize);
        }

        private int GetBoardSize()
        {
            Comunicator.Talking("Lets create some board.");
            return int.Parse(Comunicator.GetWord("Board size - "));
        }

        public void removePawn()
        {
            string coordinates = Comunicator.GetWord("Provide coordinates for pawn to be removed in a1/A1 format: ");
            this.row = int.Parse(coordinates[1].ToString()) - 1;
            this.col = ((int)char.Parse(coordinates[0].ToString().ToUpper())) - 65;
            pawn[row, col] = "_";
        }

        public void removePawn(string coordinates)
        {
            this.row = int.Parse(coordinates[1].ToString()) - 1;
            this.col = ((int)char.Parse(coordinates[0].ToString().ToUpper())) - 65;
            pawn[row, col] = "_";
        }

        public void movePawn()
        {
            string startCoordinates = Comunicator.GetWord("Provide coordinates for pawn to be moved from in a1/A1 format: ");
            this.row = int.Parse(startCoordinates[1].ToString()) - 1;
            this.col = ((int)char.Parse(startCoordinates[0].ToString().ToUpper())) - 65;
            string chosenPawn = pawn[row, col];
            string stopCoordinates = Comunicator.GetWord("Provide coordinates for pawn to be moved to in a1/A1 format: ");
            pawn[row, col] = "_";
            this.row = int.Parse(stopCoordinates[1].ToString()) - 1;
            this.col = ((int)char.Parse(stopCoordinates[0].ToString().ToUpper())) - 65;
            pawn[row, col] = chosenPawn;
        }

        private string[,] FillBoard(Player firstPlayer, Player secondPlayer, int boardSize)
        {
            string[,] pawn = new string[boardSize, boardSize];

            for (int row = 0; row < boardSize; row++)
            {
                for (int col = 0; col < boardSize; col++)
                {
                    if (row <= 1)
                    {
                        if ((col % 2 == 0 && row % 2 == 0) || (col % 2 != 0 && row % 2 != 0))
                        {
                            pawn[row, col] = firstPlayer.playerSymbol.ToString();
                            firstPlayer.itemNumber += 1;
                        }
                        else
                        {
                            pawn[row, col] = "_";
                        }
                    }

                    else if (row >= boardSize - 2)
                    {
                        if ((col % 2 == 0 && row % 2 == 0) || (col % 2 != 0 && row % 2 != 0))
                        {
                            pawn[row, col] = secondPlayer.playerSymbol.ToString();
                            secondPlayer.itemNumber += 1;
                        }
                        else
                        {
                            pawn[row, col] = "_";
                        }
                    }
                    else
                    {
                        pawn[row, col] = "_";
                    }
                }
            }
            firstPlayer.startItems = firstPlayer.itemNumber;
            secondPlayer.startItems = secondPlayer.itemNumber;
            return pawn;
        }
    }
}
/*
 _ _ _ _ _ _ _ _
|X|_|X|_|X|_|X|_|
|_|X|_|X|_|X|_|X|
|_|_|_|_|_|_|_|_|
|_|_|_|_|_|_|_|_|
|_|_|_|_|_|_|_|_|
|_|_|_|_|_|_|_|_|
|O|_|O|_|O|_|O|_|
|_|O|_|O|_|O|_|O|

*/