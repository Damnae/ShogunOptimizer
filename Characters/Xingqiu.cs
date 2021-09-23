namespace ShogunOptimizer.Characters
{
    public class Xingqiu : Character
    {
        public Xingqiu()
        {
            BaseHp = 10222;
            BaseAtk = 202;
            BaseDef = 758;

            AscensionStat = .24;
            AscensionStatType = StatType.AtkPercent;
        }

        public const string PropertySkillA = "skillA";
        public const string PropertySkillB = "skillB";
        public const string PropertySkillADuringBurst = "skillADuringBurst";
        public const string PropertySkillBDuringBurst = "skillBDuringBurst";
        public const string PropertySkillHeal = "skillHeal";
        public const string PropertyBurst = "Burst";

        public override double Calculate(string property, Build build, HitType hitType, Enemy enemy)
        {
            switch (property)
            {
                case PropertySkillA:
                    return 1.68 * GetTalentPercentageScaling(SkillLevel) * GetAtk(build) * GetMultiplier(build, DamageType.Skill, Element.Hydro, hitType, enemy);
                case PropertySkillB:
                    return 1.912 * GetTalentPercentageScaling(SkillLevel) * GetAtk(build) * GetMultiplier(build, DamageType.Skill, Element.Hydro, hitType, enemy);
                case PropertySkillADuringBurst:
                    return 1.5 * Calculate(PropertySkillA, build, hitType, enemy);
                case PropertySkillBDuringBurst:
                    return 1.5 * Calculate(PropertySkillB, build, hitType, enemy);
                case PropertySkillHeal:
                    return .06 * GetMaxHp(build) * (1 + GetStat(StatType.HealBonus, build));
                case PropertyBurst:
                    return .5427 * GetTalentPercentageScaling(BurstLevel) * GetAtk(build) * GetMultiplier(build, DamageType.Burst, Element.Hydro, hitType, enemy);
            }
            return base.Calculate(property, build, hitType, enemy);
        }

        public override double CalculateStat(StatType statType, Build build)
        {
            var stat = base.CalculateStat(statType, build);

            switch (statType)
            {
                case StatType.HydroDmgBonus:
                    stat += .2;
                    break;

                case StatType.HydroResShred when Constellation >= 2:
                    stat += .15;
                    break;
            }

            return stat;
        }
    }
}
