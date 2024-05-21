using HW01_2024.Interfaces;
using System;
using System.Collections.Generic;

namespace HW01_2024
{
    public class Game: IGame
    {

        public static readonly Func<FImon>[] FImonMakers = [
                () => new WaterFImon(4, 6, 4, "Aquafin"),
                () => new FireFImon(6, 4, 4, "Embergeist"),
                () => new LeafFImon(5, 6, 4, "Photosprout"),
                () => new WaterFImon(3, 6, 5, "Splashback"),
                () => new FireFImon(2, 10, 3, "Pyroclaw"),
                () => new LeafFImon(6, 10, 2, "Petalshade"),
                () => new LeafFImon(15, 15, 5, "ronnie coleman"),
                () => new WaterFImon(2, 9, 5, "Maritide"),
                () => new FireFImon(2, 15, 4, "Incendrake"),
                () => new LeafFImon(6, 1, 5, "Sylvasprint"),
        ];

        Trainer User = new Trainer("User", 0);
        Trainer[] NPCS = [new Trainer("Miro", 0), new Trainer("Jožo", 100), new Trainer("Maroš", 200)];
        int current = 0;

        public Game(List<int> fimons) 
        { 
            User.PickCharacters(fimons);
            NPCS[0].PickCharacters([7, 8, 9]);
            NPCS[1].PickCharacters([1, 3, 5]);
            NPCS[2].PickCharacters([0, 2, 1]);
        }
        void fight(Trainer curTrainer)
        {

            if (Duel.TrainerDuel(User, curTrainer) == 1)
            {
                Printer.PrintLose(curTrainer);
                User.LevelUp(199);
            }
            else
            {
                // User has won
                User.LevelUp(499);
                current++;
                Printer.PrintWin();
            }
        }
        void parseSort(ref List<int> positions)
        {
            while (positions.Count == 0)
            {
                var strs = Console.ReadLine().Split(' ');
                for (int i = 0; i < strs.Length; ++i)
                {
                    if (!int.TryParse(strs[i], out var num) || positions.Contains(num) || num > 2)
                    {
                        Printer.PrintError();
                        positions.RemoveAll(x => true);
                        break;
                    }
                    positions.Add(num);
                }
            }
        }

        public bool Start()
        {
            string input;
            while ((input = Console.ReadLine()) != "quit")
            {
                if (input == "info")
                {
                    Printer.PrintTrainer(NPCS[current]);
                }
                else if (input == "check")
                {
                    Printer.PrintTrainer(User);
                }
                else if (input == "sort")
                {
                    Printer.PrintTrainer(User);
                    Printer.PrintChoose();

                    var positions = new List<int>();
                    parseSort(ref positions);

                    User.ChangePositions(positions);
                }
                else if (input == "fight")
                {
                    Trainer curTrainer = NPCS[current];

                    Printer.PrintLine();
                    fight(curTrainer);
                    Printer.PrintLine();

                    if (current == NPCS.Length)
                        return true;
                }
                else if (input == "help")
                {
                    Printer.PrintHelp();
                }
                else if (input == "opponents")
                {
                    foreach (var NPC in NPCS) { Printer.PrintTrainer(NPC); }
                }
                else
                {
                    Printer.PrintWrongInput();
                }
            }
            return false;
        }
    }
}
