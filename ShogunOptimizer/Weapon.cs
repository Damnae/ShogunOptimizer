using System;

namespace ShogunOptimizer
{
    public class Weapon
    {
        public double BaseAtk;
        public readonly int Refine;

        public Tuple<StatType, double>[] Stats;
        
        public Weapon(int refine)
        {
            Refine = refine;
        }

        public virtual double GetStat(StatType statType, Build build, Character character)
        {
            return 0;
        }
    }
}
