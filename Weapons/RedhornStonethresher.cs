using System;

namespace ShogunOptimizer.Weapons
{
    public class RedhornStonethresher : Weapon
    {
        public RedhornStonethresher(int refine = 1) : base(refine)
        {
            BaseAtk = 542;
            Stats = new Tuple<StatType, double>[]
            {
                new(StatType.CritDamage, .882),
                new(StatType.DefPercent, 0.21 + .07 * Refine),
            };
            Type = WeaponType.Claymore;
        }

        public override double GetStat(StatType statType, Build build, Character character)
        {
            if (statType == StatType.AttackExtraDmg || statType == StatType.ChargedExtraDmg)
                return (.30 + .10 * Refine) * character.GetDef(build);

            return 0;
        }
    }
}
