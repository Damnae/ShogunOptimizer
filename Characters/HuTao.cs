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

        private readonly double[] ElementalSkillAttackBuff = new[] { .0384, .0407, .0430, .0460, .0483, .0506, .0536, .0566, .0596, .0626, .0656, .0685, .0715 };
        private readonly double[] ChargedAttackScaling = new[] { 1.3596, 1.4523, 1.545, 1.6686, 1.7613, 1.8695, 2.0085, 2.1476, 2.2866, 2.4257, 2.5647, 2.7038, 2.8428 };

        public override double Calculate(string property, Build build, HitType hitType, Enemy enemy)
        {
            var normalAttackElement = ElementalSkillActive ? Element.Pyro : Element.Physical;

            switch(property)
            {
                case PropertyCharged: return ChargedAttackScaling[AttackLevel - 1] * GetAtk(build) * GetMultiplier(build, DamageType.Charged, normalAttackElement, hitType, enemy);
            }

            return base.Calculate(property, build, hitType, enemy);
        }

        public override double CalculateStat(StatType statType, Build build)
        {
            var stat = base.CalculateStat(statType, build);

            switch(statType)
            {
                case StatType.AtkFlat when ElementalSkillActive:
                    stat += Math.Min(4.0 * GetBaseAtk(build), ElementalSkillAttackBuff[SkillLevel - 1] * GetMaxHp(build));
                    break;

                case StatType.PyroDmgBonus when Under50PercentHp:
                    stat += .33;
                    break;
            }

            return stat;
        }
    }
}
