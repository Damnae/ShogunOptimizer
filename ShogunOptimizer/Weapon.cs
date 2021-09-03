using System;

namespace ShogunOptimizer
{
    public class Weapon
    {
        public double BaseAtk;

        public Tuple<StatType, double>[] Stats;

        public virtual double GetStat(StatType statType, Build build, Character character)
        {
            return 0;
        }
    }
}
