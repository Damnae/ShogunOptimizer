using System;

namespace ShogunOptimizer.Weapons
{
    public class EngulfingLightning : Weapon
    {
        public EngulfingLightning(int refine = 1)
        {
            Refine = refine;
            BaseAtk = 608;
            Stats = new Tuple<StatType, double>[]
            {
                new(StatType.EnergyRecharge, .551 ),
                new(StatType.EnergyRecharge, .25 + .05 * Refine ),
            };
        }

        public override double GetStat(StatType statType, Build build, Character character)
        {
            if (statType == StatType.AtkPercent)
            {
                var atkMultiplier = .21 + .007 * Refine;
                var maximumBonus = .6 + .2 * Refine;

                return Math.Min(maximumBonus, atkMultiplier * character.GetStat(StatType.EnergyRecharge, build));
            }

            return 0;
        }
    }
}
