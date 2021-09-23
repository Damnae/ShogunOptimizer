using System;

namespace ShogunOptimizer.Weapons
{
    public class SacrificialSword : Weapon
    {
        public SacrificialSword(int refine = 1) : base(refine)
        {
            BaseAtk = 454;
            Stats = new Tuple<StatType, double>[]
            {
                new(StatType.EnergyRecharge, .613 ),
            };
            Type = WeaponType.Sword;
        }
    }
}
