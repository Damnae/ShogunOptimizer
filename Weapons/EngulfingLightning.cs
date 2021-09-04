using System;

namespace ShogunOptimizer.Weapons
{
    public class EngulfingLightning : Weapon
    {
        public EngulfingLightning(int refine = 1) : base(refine)
        {
            BaseAtk = 608;
            Stats = new Tuple<StatType, double>[]
            {
                new(StatType.EnergyRecharge, .551 ),
                new(StatType.EnergyRecharge, .25 + .05 * Refine),
            };
        }

        public override double GetStat(StatType statType, Build build, Character character)
        {
            if (statType == StatType.AtkPercent)
                return Math.Min(.6 + .2 * Refine, (.21 + .007 * Refine) * character.GetStat(StatType.EnergyRecharge, build));

            return 0;
        }
    }
}
