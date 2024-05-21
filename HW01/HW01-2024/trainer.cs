
using System;
using System.Collections.Generic;
using System.Linq;

namespace HW01_2024
{
    public class Trainer
    {
        public List<FImon> PickedFImons = new List<FImon>();
        public string Name { get; init; }
        int DefaultXp;
        public Trainer(string name, int defaultxp) { Name = name ?? "BORING DEFAULT NAME"; DefaultXp = defaultxp; }
        public int FImons { get { return PickedFImons.Count;  } }

        public void LevelUp(int max)
        {
            var Gen = new Random();

            foreach (var FImon in PickedFImons)
            {
                FImon.Buff(Gen.Next(max)); //max 4 levels per round won for each FImon separately (for fun) also this property takes care of the damage and health buffs
                FImon.RestoreHp();
            }
        }

        public void HealFImons()
        {
            foreach (var FImon in PickedFImons)
            {
                FImon.RestoreHp();
            }
        }
        public void PickCharacters(List<int> arr)
        {
            foreach (var item in arr)
            {
                var fimon = Game.FImonMakers[item]();
                fimon.Buff(DefaultXp); //defaultxp is used only on the FImons that NPCs have
                PickedFImons.Add(fimon);
            }
        }

        public void ChangePositions(List<int> arr)
        {
            var newarr = new List<FImon>();

            foreach (var item in arr)
            {
                newarr.Add(PickedFImons[item]);
            }
            PickedFImons = newarr;
        }

    }
}
