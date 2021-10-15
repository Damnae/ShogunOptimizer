﻿using System;

namespace ShogunOptimizer.Weapons
{
    public class RedhornStonethresher : Weapon
    {
        public RedhornStonethresher(int refine = 1) : base(refine)
        {
            BaseAtk = 608;
            Stats = new Tuple<StatType, double>[]
            {
                new(StatType.CritDamage, .662),
                new(StatType.DefPercent, 0.15 + .05 * Refine),
            };
            Type = WeaponType.Claymore;
        }

        public override double GetStat(StatType statType, Build build, Character character)
        {
            if (statType == StatType.AttackExtraDmg || statType == StatType.ChargedExtraDmg)
                return (.21 + .07 * Refine) * character.GetDef(build);

            return 0;
        }
    }
}