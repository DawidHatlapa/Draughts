using System;
using Microsoft.VisualBasic;
using System.Windows.Forms;

namespace polish_draughts_csharp_Boner1975
{
    class Program
    {
        public static string[] types = new string[] { "PlayerVsPlayer", "PlayerVsAi", "AiVsAi" };
        public static void Main()
        {
            Start();
            Console.ReadLine();
        }

        public static void Start()
        {
            Player firstPlayer;
            Player secondPlayer;

            switch (GetGameType(types))
            {
                case 0:
                    firstPlayer = new Player();
                    secondPlayer = new Player();
                    break;
                case 1:
                    firstPlayer = new Player();
                    secondPlayer = new Player("AI", firstPlayer.playerSymbol);
                    break;
                case 2:
                    firstPlayer = new Player("AI1");
                    secondPlayer = new Player("AI2", firstPlayer.playerSymbol);
                    break;
                default:
                    firstPlayer = new Player();
                    secondPlayer = new Player();
                    break;
            }
            Game game = new Game(firstPlayer, secondPlayer, new Board(firstPlayer, secondPlayer));
            game.Playing();
        }

        public static int GetGameType(string[] types)
        {
            while (true) { // 
                Console.Clear();
                Comunicator.Talking("Lets get type of game.");
                for (int i = 0; i < types.Length; i++)
                {
                    Console.WriteLine("\t{0}. {1}", i + 1, types[i]);
                }

                switch (int.Parse(Comunicator.GetWord("Type - ")))
                {
                    case 1:
                        return 0;
                    case 2:
                        return 1;
                    case 3:
                        return 2;
                    default:
                        continue;
                }
            }
        }
    }
}