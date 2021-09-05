using System;

namespace ShogunOptimizer.Weapons
{
    public class StaffOfHoma : Weapon
    {
        public bool Under50PercentHp;

        public StaffOfHoma(int refine = 1) : base(refine)
        {
            BaseAtk = 608;
            Stats = new Tuple<StatType, double>[]
            {
                new(StatType.CritDamage, .662),
                new(StatType.HpPercent, 0.15 + .05 * Refine),
            };
            Type = WeaponType.Spear;
        }

        public override double GetStat(StatType statType, Build build, Character character)
        {
            if (statType == StatType.AtkFlat)
                return (Under50PercentHp ? (.016 + .004 * Refine) : (.006 + .002 * Refine)) * character.GetMaxHp(build);

            return 0;
        }
    }
}
