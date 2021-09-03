using System;

namespace ShogunOptimizer.Weapons
{
    public class TheCatch : Weapon
    {
        public TheCatch()
        {
            BaseAtk = 510;
            Stats = new Tuple<StatType, double>[]
            {
                new(StatType.EnergyRecharge, .459 ),
                new(StatType.BurstDmgBonus, .32 ),
                new(StatType.BurstCritRateBonus, .12 ),
            };
        }
    }
}
