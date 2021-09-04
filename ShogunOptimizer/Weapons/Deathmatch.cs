using System;

namespace ShogunOptimizer.Weapons
{
    public class Deathmatch : Weapon
    {
        public Deathmatch(int refine = 1) : base(refine)
        {
            BaseAtk = 454;
            Stats = new Tuple<StatType, double>[]
            {
                new(StatType.EnergyRecharge, .368 ),
                new(StatType.AtkPercent, .018 + .006 * Refine),
            };
        }
    }
}
