using HW01_2024.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW01_2024
{
    public class Duel : IBattle
    {
        public static bool timeBased = false;

        /****************TIME BASED FIGHTS******************/
        public static int TimeBasedTrainerDuel(Trainer t1, Trainer t2)
        {
            int t1FImonPos = t1.PickedFImons.FindIndex(0, a => a.Alive); //i think that its always 0 because of healing at the end 
            int t2FImonPos = t2.PickedFImons.FindIndex(0, a => a.Alive); //i left it like this because the task said so

            double time = 0, f1time = 0, f2time = 0;
            for (int round = 1; t1FImonPos < t1.FImons && t2FImonPos < t2.FImons;)
            {
                Printer.PrintRound(round++);
                var (t1Res, t2Res) = TimeBasedFImonDuel(t1.PickedFImons[t1FImonPos], t2.PickedFImons[t2FImonPos], ref time, ref f1time, ref f2time);
                t1FImonPos += t1Res;
                t2FImonPos += t2Res;
            }
            string winner = (t1FImonPos < t1.FImons ? t1 : t2).Name;
            Printer.PrintWinner(winner);

            t1.HealFImons();
            t2.HealFImons();

            return t1FImonPos < t1.FImons ? 0 : 1; //this means that t1s FImons stayed alive
        }

        public (int, int) TimeBasedFImonDuel(FImon f1, FImon f2)
        {
            double a = 0, b = 0;
            double time = 0;
            return TimeBasedFImonDuel(f1, f2, ref time, ref a, ref b);
        }

        public static (int, int) TimeBasedFImonDuel(FImon f1, FImon f2, ref double time, ref double f1time, ref double f2time)
        {
            Printer.PrintFImon(f1, false);
            Printer.PrintFImon(f2, false);
            Console.WriteLine();
            while (f1.Alive && f2.Alive)
            {
                if ((f1time + 1.0 / f1.AttackSpeed - time) < (f2time + 1.0 / f2.AttackSpeed - time))
                {
                    f1time += 1.0 / f1.AttackSpeed;
                    time = f1time;
                    Printer.PrintAttack(f1.Attack(f2));
                }
                else if ((f1time + 1.0 / f1.AttackSpeed - time) > (f2time + 1.0 / f2.AttackSpeed - time))
                {
                    f2time += 1.0 / f2.AttackSpeed;
                    time = f2time;
                    Printer.PrintAttack(f2.Attack(f1));
                }
                else
                {
                    f1time += 1.0 / f1.AttackSpeed;
                    f2time += 1.0 / f2.AttackSpeed;
                    time += f1time;
                    Printer.PrintAttack(f1.Attack(f2));
                    Printer.PrintAttack(f2.Attack(f1));
                }
                Printer.PrintFImon(f1, false);
                Printer.PrintFImon(f2, false);
                Console.WriteLine();
            }
            f1time = f2time = time;
            return (f1.Alive ? 0 : 1, f2.Alive ? 0 : 1);
        }

        /****************NORMAL FIGHTS******************/

        public static int NormalTrainerDuel(Trainer t1, Trainer t2)
        {
            int t1FImonPos = t1.PickedFImons.FindIndex(0, a => a.Alive); //i think that its always 0 because of healing at the end 
            int t2FImonPos = t2.PickedFImons.FindIndex(0, a => a.Alive); //i left it like this because the task said so

            double time = 0, f1time = 0, f2time = 0;
            for (int round = 1; t1FImonPos < t1.FImons && t2FImonPos < t2.FImons;)
            {
                Printer.PrintRound(round++);
                var (t1Res, t2Res) = NormalFImonDuel(t1.PickedFImons[t1FImonPos], t2.PickedFImons[t2FImonPos]);
                t1FImonPos += t1Res;
                t2FImonPos += t2Res;
            }
            string winner = (t1FImonPos < t1.FImons ? t1 : t2).Name;
            Printer.PrintWinner(winner);

            t1.HealFImons();
            t2.HealFImons();

            return t1FImonPos < t1.FImons ? 0 : 1; //this means that t1s FImons stayed alive
        }
        public static (int, int) NormalFImonDuel(FImon f1, FImon f2)
        {
            Printer.PrintFImon(f1, false);
            Printer.PrintFImon(f2, false);
            Console.WriteLine();

            FImon first, second;
            if (f1.AttackSpeed >= f2.AttackSpeed)
            {
                first = f1;
                second = f2;
            }
            else
            {
                first = f2;
                second = f1;
            }

            while (f1.Alive && f2.Alive)
            {
                Printer.PrintAttack(first.Attack(second));
                Printer.PrintFImon(f1, false);
                Printer.PrintFImon(f2, false);
                Console.WriteLine();
                var tmp = first;
                first = second;
                second = tmp;
            }

            return (f1.Alive ? 0 : 1, f2.Alive ? 0 : 1);
        }

        public static int TrainerDuel(Trainer t1, Trainer t2)
        {
            if (timeBased)
            {
                return TimeBasedTrainerDuel(t1, t2);
            }
            else
            {
                return NormalTrainerDuel(t1, t2);
            }
        }
    }
}
