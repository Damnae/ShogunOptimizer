using System;

namespace ShogunOptimizer.Weapons
{
    public class TheCatch : Weapon
    {
        public TheCatch(int refine = 1) : base(refine)
        {
            BaseAtk = 510;
            Stats = new Tuple<StatType, double>[]
            {
                new(StatType.EnergyRecharge, .459 ),
                new(StatType.BurstDmgBonus, .12 + .04 * Refine),
                new(StatType.BurstCritRateBonus, .045 + .015 * Refine),
            };
            Type = WeaponType.Spear;
        }
    }
}
