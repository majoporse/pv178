using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace HW01_2024
{
    class Printer
    {
        static string HelpString = @"---------------------
Controls:
To start the game type: start <position> <position> <position>
To check your FImons type: check
To check your opponents FImons type: info
To change the attack order of your FImons type: sort
After typing sort type the positions as following: <position1> <position2> <position3>
If you are ready to fight your opponent type: fight
To quit the game type: quit
If you want to see controls again, type: help
For testing purposes there is 6th character that wins all (basically)
---------------------
";
        public static readonly (int, int, int, string, string)[] FImonDesc = [
            (4, 6, 4, "Aquafin", "Water"),
            (6, 4, 4, "Embergeist", "Fire"),
            (5, 6, 4, "Photosprout", "Leaf"),
            (3, 6, 5, "Splashback", "Water"),
            (2, 10, 3, "Pyroclaw", "Fire"),
            (6, 10, 2, "Petalshade", "Leaf"),
        ];
        public static void PrintRound(int r) { Console.WriteLine($"[[[[[.....ROUND {r} START.....]]]]]"); }
        public static void PrintHelp() { Console.WriteLine(HelpString); }
        public static void PrintDesc()
        {
            Console.Write(HelpString);
            foreach (var FImon in FImonDesc)
            {
                Console.WriteLine($"Name: {FImon.Item4,12} | Attack damage: {FImon.Item1,3} | Health: {FImon.Item2,3} | Attack speed: {FImon.Item3,3} | Type: {FImon.Item5}");
            }
            Console.WriteLine("---------------------");
        }

        public static void PrintTrainer(Trainer trainer)
        {
            Console.WriteLine($"{trainer.Name}:");
            Console.WriteLine("---------------------");
            foreach (var FImon in trainer.PickedFImons)
            {
                PrintFImon(FImon);
            }
            Console.WriteLine("---------------------");
        }

        public static void PrintWinner(string winner) { Console.WriteLine($"[[[...{winner} has won the round...]]]"); }

        public static void PrintAttack(string s) { Console.WriteLine($"|| {s}"); }

        public static void PrintLose(Trainer curTrainer) { Console.WriteLine($"you lost against: {curTrainer.Name}, you recieve up to 199xp"); }

        public static void PrintWin() { Console.WriteLine("You won! You recieve up to 499 xp!"); }

        public static void PrintLine() { Console.WriteLine("---------------------"); }

        public static void PrintError() { Console.WriteLine("Error parsing! Try again!"); }
        public static void PrintChoose() { Console.WriteLine("Choose your attack order!"); }
        public static void PrintErrorVal(int item) { Console.WriteLine($"error parsing value: {item}!"); }
        public static void PrintWrongInput() { Console.WriteLine("worng input, try again..."); }
        public static void PrintWinGame() { Console.WriteLine("***************YOU WON***************"); }
        public static void PrintFImon(FImon f, bool printxp = true)
        {
            Console.Write("||");
            Console.Write($" Name: {f.Name,12} |");
            Console.Write($" Attack damage: {f.AttackDamage,3} |");
            Console.Write($" Health: {f.Health,3} |");
            Console.Write($" Attack speed: {(f.AttackSpeed),3:F0} |");
            if (printxp)
            {
                Console.Write($" Level: {f.Level,3} |");
                Console.Write($" Xp: {f.Xp % 100} |");
            }
            Console.Write($" Type: {f.TypeString()}\n");
        }
    }

}
