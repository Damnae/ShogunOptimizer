using System;

namespace ShogunOptimizer.Weapons
{
    public class Whiteblind : Weapon
    {
        public Whiteblind(int refine = 1) : base(refine)
        {
            BaseAtk = 510;
            Stats = new Tuple<StatType, double>[]
            {
                new(StatType.DefPercent, .517 ),
                new(StatType.AtkPercent, 4 * (.045 + .015 * Refine)),
                new(StatType.DefPercent, 4 * (.045 + .015 * Refine)),
            };
            Type = WeaponType.Claymore;
        }
    }
}
