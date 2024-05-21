using System;
using System.Collections.Generic;
using System.Globalization;

namespace HW01_2024
{

    class Program
    {
        static int Main(string[] args)
         {
            if (args.Length == 1)
            {
                if (args[0] == "--time")
                {
                    Duel.timeBased = true;
                }
                else
                {
                    Printer.PrintError();
                    return 1;
                }   
            }
                
            Printer.PrintDesc();
            string read = Console.ReadLine();
            if (read == null || !read.StartsWith("start "))
                return 1;

            var splitted = read.Substring(6).Split(' ');
            if (splitted.Length != 3)
            {
                Printer.PrintError();
                return 1;
            }
            List<int> ints = new List<int>();

            foreach (var item in splitted)
            {
                if (!int.TryParse(item, out var value))
                {
                    Printer.PrintErrorVal(value);
                    return 1;
                }
                ints.Add(value);
            }

            Game g = new Game(ints);
            if (g.Start())
                Printer.PrintWinGame();

            return 0;
        }
    }
}
