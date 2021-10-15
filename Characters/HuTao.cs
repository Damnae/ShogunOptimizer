using System;

namespace ShogunOptimizer.Characters
{
    public class HuTao : Character
    {
        public bool ElementalSkillActive;
        public bool Under50PercentHp;

        public HuTao()
        {
            BaseHp = 15552;
            BaseAtk = 107;
            BaseDef = 876;

            AscensionStat = .384;
            AscensionStatType = StatType.CritDamage;
        }

        public const string PropertyCharged = "charged";

        public override double Calculate(string property, Build build, HitType hitType, Enemy enemy)
        {
            switch (property)
            {
                case PropertyCharged:
                    return CalculateDamage(build, 1.3596 * GetTalentPercentageScaling(SkillLevel) * GetAtk(build), DamageType.Charged, ElementalSkillActive ? Element.Pyro : Element.Physical, hitType, enemy);
            }
            return base.Calculate(property, build, hitType, enemy);
        }

        public override double CalculateStat(StatType statType, Build build)
        {
            var stat = base.CalculateStat(statType, build);

            switch (statType)
            {
                case StatType.AtkFlat when ElementalSkillActive:
                    stat += Math.Min(4.0 * GetBaseAtk(build), .0384 * GetTalentPercentageScaling(SkillLevel) * GetMaxHp(build));
                    break;

                case StatType.PyroDmgBonus when Under50PercentHp:
                    stat += .33;
                    break;
            }

            return stat;
        }
    }
}
