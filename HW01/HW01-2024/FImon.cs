using System;
using System.Xml.Linq;
using System.Xml.Schema;

namespace HW01_2024
{
    public abstract class FImon
    {
        public int Health{ get; set; }
        int maxHealth;
        public int AttackDamage{ get; set; }
        public int AttackSpeed{ get; init; }
        public int Xp { get; private set; }
        public int Level { get; private set; }
        Random Gen = new Random();

        public void Buff(int value)
        {
            Xp += value;
            int LvlDiff = (Xp - Level * 100) / 100;
            Level = Xp / 100;
            
            maxHealth += Gen.Next(1,3) * LvlDiff;
            AttackDamage += Gen.Next(1, 3) * LvlDiff;
        }

        public void RestoreHp() { Health = maxHealth; }

        public string? Name { get; set; }
        public bool Alive { get { return Health > 0; } }

        public FImon(int health, int attackDamage, int attackSpeed, string name)
        {
            maxHealth = health;
            Health = maxHealth;
            AttackDamage = attackDamage;
            AttackSpeed = attackSpeed;
            Name = name;
            Xp = 100;
            Level = 1;
        }

        public abstract string Attack(FImon f);
        public abstract string TypeString();
        private string BasicAttack(FImon f) 
        {
            Health -= f.AttackDamage;
            return $"FImon: {f.Name,12} just dealt { f.AttackDamage} damage to {Name}";
        }

        protected string EnhancedAttack(FImon f)
        {
            Health -= 2 * f.AttackDamage;
            return $"FImon: {f.Name,12} just dealt critical strike for {2 * f.AttackDamage} to {Name}";
        }

        public virtual string RecieveAttack(LeafFImon f) { return BasicAttack(f); }
        public virtual string RecieveAttack(FireFImon f) { return BasicAttack(f); }
        public virtual string RecieveAttack(WaterFImon f) { return BasicAttack(f); }
    }

    public class LeafFImon : FImon
    {
        public override string TypeString() { return "Leaf"; }
        public override string Attack(FImon f) { return f.RecieveAttack(this); }
        public override string RecieveAttack(FireFImon f) { return EnhancedAttack(f);  }
            
        public LeafFImon( int attackDamage, int health, int attackSpeed, string name) : base(health, attackDamage, attackSpeed, name) { }
    }

    public class WaterFImon: FImon
    {
        public override string TypeString() { return "Water"; }
        public override string Attack(FImon f) { return f.RecieveAttack(this); }

        public override string RecieveAttack(LeafFImon f) { return EnhancedAttack(f); }
        public WaterFImon( int attackDamage, int health, int attackSpeed, string name) : base(health, attackDamage, attackSpeed, name) { }
    }

    public class FireFImon : FImon
    {
        public override string TypeString() { return "Fire"; }
        public override string Attack(FImon f) { return f.RecieveAttack(this); }
        public override string RecieveAttack(WaterFImon f) { return EnhancedAttack(f); }
        public FireFImon( int attackDamage, int health, int attackSpeed, string name): base(health, attackDamage, attackSpeed, name) { }
    }
}
