namespace ShogunOptimizer.Characters
{
    public class Kokomi : Character
    {
        public Kokomi()
        {
            BaseHp = 13471;
            BaseAtk = 234;
            BaseDef = 657;

            AscensionStat = .288;
            AscensionStatType = StatType.HydroDmgBonus;

            Stats[(int)StatType.HealBonus] += .25;
            Stats[(int)StatType.CritRate] -= 1;
        }

        public const string PropertyAttack1 = "attack1";
        public const string PropertyAttack2 = "attack2";
        public const string PropertyAttack3 = "attack3";
        public const string PropertyCharged = "charged";
        public const string PropertySkillDamage = "skillDamage";
        public const string PropertySkillHealing = "skillHealing";
        public const string PropertyBurstInitial = "burstInitial";
        public const string PropertyBurstAttack1 = "burstAttack1";
        public const string PropertyBurstAttack2 = "burstAttack2";
        public const string PropertyBurstAttack3 = "burstAttack3";
        public const string PropertyBurstAttack3C1 = "burstC1";
        public const string PropertyBurstCharged = "burstCharged";
        public const string PropertyBurstSkillDamage = "burstSkillDamage";
        public const string PropertyBurstHealing = "burstHealing";

        public override double Calculate(string property, Build build, HitType hitType, Enemy enemy)
        {
            switch (property)
            {
                case PropertyAttack1: return (0.6838 * GetTalentAttackScaling(AttackLevel)) * GetAtk(build) * GetMultiplier(build, DamageType.Normal, Element.Hydro, hitType, enemy);
                case PropertyAttack2: return (0.6154 * GetTalentAttackScaling(AttackLevel)) * GetAtk(build) * GetMultiplier(build, DamageType.Normal, Element.Hydro, hitType, enemy);
                case PropertyAttack3: return (0.9431 * GetTalentAttackScaling(AttackLevel)) * GetAtk(build) * GetMultiplier(build, DamageType.Normal, Element.Hydro, hitType, enemy);
                case PropertyCharged: return (1.4832 * GetTalentAttackScaling(AttackLevel)) * GetAtk(build) * GetMultiplier(build, DamageType.Charged, Element.Hydro, hitType, enemy);

                case PropertySkillDamage: return (1.0919 * GetTalentPercentageScaling(SkillLevel)) * GetAtk(build) * GetMultiplier(build, DamageType.Skill, Element.Hydro, hitType, enemy);
                case PropertySkillHealing: return (0.044 * GetTalentPercentageScaling(SkillLevel)) * GetMaxHp(build) + (424 * GetTalentFlatScaling(SkillLevel)) * (1 + GetStat(StatType.HealBonus, build));

                case PropertyBurstInitial: return (0.1042 * GetTalentPercentageScaling(BurstLevel)) * GetMaxHp(build) * GetMultiplier(build, DamageType.Burst, Element.Hydro, hitType, enemy);
                case PropertyBurstAttack1: return (0.0484 * GetTalentPercentageScaling(BurstLevel) * GetMaxHp(build)) + Calculate(PropertyAttack1, build, hitType, enemy);
                case PropertyBurstAttack2: return (0.0484 * GetTalentPercentageScaling(BurstLevel) * GetMaxHp(build)) + Calculate(PropertyAttack2, build, hitType, enemy);
                case PropertyBurstAttack3: return (0.0484 * GetTalentPercentageScaling(BurstLevel) * GetMaxHp(build)) + Calculate(PropertyAttack3, build, hitType, enemy);
                case PropertyBurstAttack3C1: return 0.3 * GetMaxHp(build) * GetMultiplier(build, DamageType.None, Element.Hydro, hitType, enemy);
                case PropertyBurstCharged: return (0.0678 * GetMaxHp(build) * GetTalentPercentageScaling(BurstLevel)) + Calculate(PropertyCharged, build, hitType, enemy);
                case PropertyBurstSkillDamage: return (0.071 * GetMaxHp(build) * GetTalentPercentageScaling(BurstLevel)) + Calculate(PropertySkillDamage, build, hitType, enemy);
                case PropertyBurstHealing: return (0.0081 * GetTalentPercentageScaling(SkillLevel)) * GetMaxHp(build) + (77 * GetTalentFlatScaling(SkillLevel)) * (1 + GetStat(StatType.HealBonus, build));

                default: return base.Calculate(property, build, hitType, enemy);
            }
        }

        public override double CalculateStat(StatType statType, Build build)
        {
            var stat = base.CalculateStat(statType, build);

            switch (statType)
            {
                case StatType.AttackDmgBonus:
                case StatType.ChargedDmgBonus:
                    stat += .15 * GetStat(StatType.HealBonus, build);
                    break;
            }

            return stat;
        }
    }
}
