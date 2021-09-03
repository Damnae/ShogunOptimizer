using System;

namespace ShogunOptimizer.Weapons
{
    public class FavoniusLance : Weapon
    {
        public FavoniusLance()
        {
            BaseAtk = 565;
            Stats = new Tuple<StatType, double>[]
            {
                new ( StatType.EnergyRecharge, .306 ),
            };
        }
    }
}
