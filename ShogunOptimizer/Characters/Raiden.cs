using System;

namespace ShogunOptimizer.Characters
{
    public class Raiden : Character
    {
        public Raiden()
        {
            BaseAtk = 337;

            AscensionStat = .32;
            AscensionStatType = StatType.EnergyRecharge;
        }

        public const string PropertyBurstInitial = "burstInitial";

        public override double Calculate(string property, Build build, HitType hitType, Enemy enemy)
        {
            switch (property)
            {
                case PropertyBurstInitial:
                    return (7.2144 + 0.07 * 60) * GetDamage(build, DamageType.Burst, Element.Electro, hitType, enemy);

                default:
                    return base.Calculate(property, build, hitType, enemy);
            }
        }

        public override double GetStat(StatType statType, Build build)
        {
            var stat = base.GetStat(statType, build);

            switch (statType)
            {
                case StatType.ElectroDmgBonus:
                    stat += .4 * Math.Max(0, base.GetStat(StatType.EnergyRecharge, build) - 1);
                    break;
                case StatType.DefShred when Constellation >= 2:
                    stat += .6;
                    break;
            }

            return stat;
        }
    }
}
