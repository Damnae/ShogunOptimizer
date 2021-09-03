using System;

namespace ShogunOptimizer.Weapons
{
    public class EngulfingLightning : Weapon
    {
        public EngulfingLightning()
        {
            BaseAtk = 608;
            Stats = new Tuple<StatType, double>[]
            {
                new(StatType.EnergyRecharge, .551 ),
                new(StatType.EnergyRecharge, .3 ),
            };
        }

        public override double GetStat(StatType statType, Build build, Character character)
        {
            if (statType == StatType.AtkPercent)
                return Math.Min(.80, .28 * character.GetStat(StatType.EnergyRecharge, build));

            return 0;
        }
    }
}
