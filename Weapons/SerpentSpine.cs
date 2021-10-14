using System;

namespace ShogunOptimizer.Weapons
{
    public class SerpentSpine : Weapon
    {
        public SerpentSpine(int refine = 1) : base(refine)
        {
            BaseAtk = 510;
            Stats = new Tuple<StatType, double>[]
            {
                new(StatType.CritRate, .276 ),
                new(StatType.DmgBonus, 5 * (.05 + .01 * Refine)),
            };
            Type = WeaponType.Claymore;
        }
    }
}
