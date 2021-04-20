using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace polish_draughts_csharp_Boner1975
{
    class Interface
    {
        public static string spacingForOneDigitRows = "  |";
        public static string spacingForTwoDigitRows = " |";
        public static void DrawBoard(Board board, Game game, Player player)
        {
            Console.WriteLine();
            Console.Write("    ");
            for (int i = 0; i < board.boardSize; i++)
            {
                Console.Write((char)(i + 65) + " ");
            }
            Console.WriteLine();
            Console.Write("   ");
            for (int i = 0; i < board.boardSize; i++)
            {
                Console.Write(" _");
            }
            Console.WriteLine();
            for (int row = 0; row < board.boardSize; row++)
            {

                if (row <= 8)
                {
                    Console.Write((row + 1) + spacingForOneDigitRows);
                    for (int col = 0; col < board.boardSize; col++)
                    {
                        Console.Write(board.pawn[row, col] + "|");
                    }
                    Console.WriteLine();
                }
                else
                {
                    Console.Write((row + 1) + spacingForTwoDigitRows);
                    for (int col = 0; col < board.boardSize; col++)
                    {
                        Console.Write(board.pawn[row, col] + "|");
                    }
                    Console.WriteLine();
                }
            }
            DrawInformation(game, player);
        }

        public static void DrawInformation(Game game, Player player)
        {
            int cellLength = 0;
            
            if (game.firstPlayer.playerName.ToCharArray().Length > game.secondPlayer.playerName.ToCharArray().Length)
            {
                cellLength = game.firstPlayer.playerName.ToCharArray().Length;
            }
            else
            {
                cellLength = game.secondPlayer.playerName.ToCharArray().Length;
            }


            if (cellLength <= 18)
            {
                cellLength = 18;
            }

            PrintSameOnePart(cellLength + 4, 3);
            Console.WriteLine();

            Console.Write("|");
            PritStringBySellSize(game.firstPlayer.playerName, cellLength + 4);
            Console.Write("|");
            PritStringBySellSize("Current Move", cellLength + 4);
            Console.Write("|");
            PritStringBySellSize(game.secondPlayer.playerName, cellLength + 4);
            Console.Write("|");
            Console.WriteLine();

            Console.Write("|");
            PritStringBySellSize("Destroyed items: " + (game.secondPlayer.startItems - game.secondPlayer.itemNumber).ToString(), cellLength + 4);
            Console.Write("|");
            PritStringBySellSize(player.playerName + "(" + player.playerSymbol + ")", cellLength + 4);
            Console.Write("|");
            PritStringBySellSize("Destroyed items: " + (game.firstPlayer.startItems - game.firstPlayer.itemNumber).ToString(), cellLength + 4);
            Console.Write("|");
            Console.WriteLine();

            Console.Write("|");
            PritStringBySellSize(game.firstPlayer.itemNumber.ToString() + " Items left", cellLength + 4);
            Console.Write("|");
            PritStringBySellSize(" ", cellLength + 4);
            Console.Write("|");
            PritStringBySellSize(game.secondPlayer.itemNumber.ToString() + " Items left", cellLength + 4);
            Console.Write("|");
            Console.WriteLine();

            Console.Write("|");
            PritStringBySellSize("______________________", cellLength + 4);
            Console.Write("|");
            PritStringBySellSize("______________________", cellLength + 4);
            Console.Write("|");
            PritStringBySellSize("______________________", cellLength + 4);
            Console.Write("|");
            Console.WriteLine();
        }


        public static void PritStringBySellSize(string line, int maxSize)
        {
            DrawSymbol((maxSize - line.ToCharArray().Length) / 2, " ");
            Console.Write(line);
            DrawSymbol(((maxSize - line.ToCharArray().Length) / 2) + (line.ToCharArray().Length % 2), " ");
        }

        public static void DrawSymbol(int count, string symbol)
        {
            for (int i = 0; i < count; i++)
            {
                Console.Write(symbol);
            }
        }

        public static void PrintSameOnePart(int cellLength, int printTime)
        {
            Console.Write("|");
            for (int i = 0; i < printTime; i++)
            {
                PritStringBySellSize("----------------------", cellLength);
                Console.Write("|");
            }
        }
    }
}
