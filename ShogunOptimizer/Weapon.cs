using System;

namespace ShogunOptimizer
{
    public class Weapon
    {
        public double BaseAtk;
        public int Refine = 1;

        public Tuple<StatType, double>[] Stats;

        public virtual double GetStat(StatType statType, Build build, Character character)
        {
            return 0;
        }
    }
}
