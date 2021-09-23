using System;

namespace ShogunOptimizer.Weapons
{
    public class PrimordialJadeCutter : Weapon
    {
        public PrimordialJadeCutter(int refine = 1) : base(refine)
        {
            BaseAtk = 542;
            Stats = new Tuple<StatType, double>[]
            {
                new(StatType.CritRate, .441 ),
                new(StatType.HpPercent, .15 * Refine),
            };
            Type = WeaponType.Sword;
        }

        public override double GetStat(StatType statType, Build build, Character character)
        {
            if (statType == StatType.AtkFlat)
                return (.009 + .005 * Refine) * character.GetMaxHp(build);

            return 0;
        }
    }
}
