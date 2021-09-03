using System;

namespace ShogunOptimizer.Weapons
{
    class StaffOfHoma : Weapon
    {
        public StaffOfHoma()
        {
            BaseAtk = 608;
            Stats = new Tuple<StatType, double>[]
            {
                new(StatType.CritDamage, .662 ),
                new(StatType.HpPercent, 0.20),
            };
        }

        public override double GetStat(StatType statType, Build build, Character character)
        {
            if (statType == StatType.AtkFlat)
                return .008 * character.GetMaxHealth(build);

            return 0;
        }
    }
}
