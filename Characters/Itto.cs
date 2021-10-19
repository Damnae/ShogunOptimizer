using System;

namespace ShogunOptimizer.Characters
{
    public class Itto : Character
    {
        public bool BurstActive;

        public Itto()
        {
            BaseHp = 13707;
            BaseAtk = 191;
            BaseDef = 930;

            AscensionStat = .242;
            AscensionStatType = StatType.CritRate;
        }

        public const string PropertyAttack1 = "attack1";
        public const string PropertyAttack2 = "attack2";
        public const string PropertyAttack3 = "attack3";
        public const string PropertyAttack4 = "attack4";
        public const string PropertyChargedCombo = "chargedCombo";
        public const string PropertyChargedFinal = "chargedFinal";
        public const string PropertyChargedSingle = "chargedSingle";
        public const string PropertySkill = "skill";

        public override double Calculate(string property, Build build, HitType hitType, Enemy enemy)
        {
            switch (property)
            {
                case PropertyAttack1: return CalculateDamage(build, .7923 * GetTalentPercentageScaling(AttackLevel) * GetAtk(build), DamageType.Normal, BurstActive ? Element.Geo : Element.Physical, hitType, enemy);
                case PropertyAttack2: return CalculateDamage(build, .7637 * GetTalentPercentageScaling(AttackLevel) * GetAtk(build), DamageType.Normal, BurstActive ? Element.Geo : Element.Physical, hitType, enemy);
                case PropertyAttack3: return CalculateDamage(build, .9164 * GetTalentPercentageScaling(AttackLevel) * GetAtk(build), DamageType.Normal, BurstActive ? Element.Geo : Element.Physical, hitType, enemy);
                case PropertyAttack4: return CalculateDamage(build, 1.1722 * GetTalentPercentageScaling(AttackLevel) * GetAtk(build), DamageType.Normal, BurstActive ? Element.Geo : Element.Physical, hitType, enemy);

                case PropertyChargedCombo: return CalculateDamage(build, .9126 * GetTalentPercentageScaling(AttackLevel) * GetAtk(build) + .35 * GetDef(build), DamageType.Charged, BurstActive ? Element.Geo : Element.Physical, hitType, enemy);
                case PropertyChargedFinal: return CalculateDamage(build, 1.9092 * GetTalentPercentageScaling(AttackLevel) * GetAtk(build) + .35 * GetDef(build), DamageType.Charged, BurstActive ? Element.Geo : Element.Physical, hitType, enemy);
                case PropertyChargedSingle: return CalculateDamage(build, .9047 * GetTalentPercentageScaling(AttackLevel) * GetAtk(build), DamageType.Charged, BurstActive ? Element.Geo : Element.Physical, hitType, enemy);

                case PropertySkill: return CalculateDamage(build, 3.072 * GetTalentPercentageScaling(SkillLevel) * GetAtk(build), DamageType.Skill, Element.Geo, hitType, enemy);

                default: return base.Calculate(property, build, hitType, enemy);
            }
        }

        public override double CalculateStat(StatType statType, Build build)
        {
            var stat = base.CalculateStat(statType, build);

            switch (statType)
            {
                case StatType.AtkFlat when BurstActive:
                    stat += .576 * GetTalentPercentageScaling(SkillLevel) * GetDef(build);
                    break;
            }

            return stat;
        }
    }
}
