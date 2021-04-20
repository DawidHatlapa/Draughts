using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace polish_draughts_csharp_Boner1975
{
    class Comunicator // This class consists of functions which make comunication with user/users 
    {
        private static string comunicatorName = "Wizard";

        public static void Talking(string word)
        {
            Console.WriteLine("{0}: {1}", Comunicator.comunicatorName, word);
        }

        public static string GetWord(string word = "")
        {
            Console.Write(word);
            return Console.ReadLine();
        }

        public static void PrintResult(Player player)
        {
            Talking("Ther winner is " + player.playerName + "\nGratulation!");
        }
    }
}
