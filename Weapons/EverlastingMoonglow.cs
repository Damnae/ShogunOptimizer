using System;

namespace ShogunOptimizer.Weapons
{
    public class EverlastingMoonglow : Weapon
    {
        public EverlastingMoonglow(int refine = 1) : base(refine)
        {
            BaseAtk = 608;
            Stats = new Tuple<StatType, double>[]
            {
                new(StatType.HpPercent, .496 ),
                new(StatType.HealBonus, .075 + .025 * Refine),
            };
            Type = WeaponType.Spear;
        }

        public override double GetStat(StatType statType, Build build, Character character)
        {
            //if (statType == StatType.AttackDmgBonus)
            //    return (.005 + .005 * Refine) * character.GetMaxHp(build);

            return 0;
        }
    }
}
