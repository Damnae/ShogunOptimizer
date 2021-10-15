using System;

namespace ShogunOptimizer.Weapons
{
    public class WolfsGravestone : Weapon
    {
        public WolfsGravestone(int refine = 1) : base(refine)
        {
            BaseAtk = 608;
            Stats = new Tuple<StatType, double>[]
            {
                new(StatType.AtkPercent, .496 + .15 + .05 * Refine),
            };
            Type = WeaponType.Claymore;
        }
    }
}
