using System;

namespace HW01_2024.Interfaces
{
    public interface IBattle
    {
        /// <summary>
        /// Performs a battle between two trainers.
        /// </summary>
        /// <param name="player">Player trainer</param>
        /// <param name="enemy">Enemy trainer</param>
        /// <returns>Winner trainer</returns>
        //TODO:
        static int TrainerDuel(Trainer t1, Trainer t2)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Performs one round of a battle between two FImons.
        /// </summary>
        /// <param name="playerFImon">Player trainer's FImon</param>
        /// <param name="enemyFImon">Enemy trainer's FImon</param>
        /// <returns>Winner FImon</returns>
        //TODO:
        static (int, int) FImonDuel(FImon f1, FImon f2)
        {
            throw new NotImplementedException();
        }
    }
}
