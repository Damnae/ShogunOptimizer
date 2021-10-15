using System;

namespace ShogunOptimizer.Characters
{
    public class Raiden : Character
    {
        public int Resolve = 60;

        public Raiden()
        {
            BaseHp = 12907;
            BaseAtk = 337;
            BaseDef = 789;

            AscensionStat = .32;
            AscensionStatType = StatType.EnergyRecharge;
        }

        public const string PropertySkillInitial = "skillInitial";
        public const string PropertySkillTick = "skillTick";
        public const string PropertyBurstInitial = "burstInitial";
        public const string PropertyBurstAttack1 = "burstAttack1";
        public const string PropertyBurstAttack2 = "burstAttack2";
        public const string PropertyBurstAttack3 = "burstAttack3";
        public const string PropertyBurstAttack4A = "burstAttack4A";
        public const string PropertyBurstAttack4B = "burstAttack4B";
        public const string PropertyBurstAttack5 = "burstAttack5";
        public const string PropertyBurstChargedA = "burstChargedA";
        public const string PropertyBurstChargedB = "burstChargedB";
        public const string PropertyBurstEnergyRestored = "burstEnergyRestored";

        public const string PropertyBurst2N4C1N2C = "burst2N4C1N2C";
        
        public override double Calculate(string property, Build build, HitType hitType, Enemy enemy)
        {
            var burstScaling = GetTalentPercentageScaling(BurstLevel);
            switch (property)
            {
                case PropertySkillInitial: return CalculateDamage(build, (1.172 * GetTalentPercentageScaling(SkillLevel)) * GetAtk(build), DamageType.Skill, Element.Electro, hitType, enemy);
                case PropertySkillTick: return CalculateDamage(build, (0.42 * GetTalentPercentageScaling(SkillLevel)) * GetAtk(build), DamageType.Skill, Element.Electro, hitType, enemy);

                case PropertyBurstInitial: return CalculateDamage(build, (4.008 * burstScaling + 0.0389 * burstScaling * Resolve) * GetAtk(build), DamageType.Burst, Element.Electro, hitType, enemy);
                
                case PropertyBurstAttack1: return CalculateDamage(build, (.4474 * burstScaling + .0073 * burstScaling * Resolve) * GetAtk(build), DamageType.Burst, Element.Electro, hitType, enemy);
                case PropertyBurstAttack2: return CalculateDamage(build, (.4396 * burstScaling + .0073 * burstScaling * Resolve) * GetAtk(build), DamageType.Burst, Element.Electro, hitType, enemy);
                case PropertyBurstAttack3: return CalculateDamage(build, (.5382 * burstScaling + .0073 * burstScaling * Resolve) * GetAtk(build), DamageType.Burst, Element.Electro, hitType, enemy);
                case PropertyBurstAttack4A: return CalculateDamage(build, (.3089 * burstScaling + .0073 * burstScaling * Resolve) * GetAtk(build), DamageType.Burst, Element.Electro, hitType, enemy);
                case PropertyBurstAttack4B: return CalculateDamage(build, (.3098 * burstScaling + .0073 * burstScaling * Resolve) * GetAtk(build), DamageType.Burst, Element.Electro, hitType, enemy);
                case PropertyBurstAttack5: return CalculateDamage(build, (.7394 * burstScaling + .0073 * burstScaling * Resolve) * GetAtk(build), DamageType.Burst, Element.Electro, hitType, enemy);
                case PropertyBurstChargedA: return CalculateDamage(build, (.616 * burstScaling + .0073 * burstScaling * Resolve) * GetAtk(build), DamageType.Burst, Element.Electro, hitType, enemy);
                case PropertyBurstChargedB: return CalculateDamage(build, (.7436 * burstScaling + .0073 * burstScaling * Resolve) * GetAtk(build), DamageType.Burst, Element.Electro, hitType, enemy);

                case PropertyBurst2N4C1N2C:
                {
                    var damage = GetAtk(build) * GetMultiplier(build, DamageType.Burst, Element.Electro, hitType, enemy);
                    var resolveBonus = .0073 * burstScaling * Resolve;
                    return (.4474 * burstScaling + resolveBonus) * damage * 3
                        + (.4396 * burstScaling + resolveBonus) * damage * 3
                        + (.5382 * burstScaling + resolveBonus) * damage * 2
                        + (.3089 * burstScaling + resolveBonus) * damage * 2
                        + (.3098 * burstScaling + resolveBonus) * damage * 2
                        + (.616 * burstScaling + resolveBonus) * damage * 3
                        + (.7436 * burstScaling + resolveBonus) * damage * 3;
                }

                case PropertyBurstEnergyRestored:
                    var baseValue = Math.Min(2.5, 1.6 + 0.1 * BurstLevel);
                    var talentMultiplier = 1 + .6 * Math.Max(0, GetStat(StatType.EnergyRecharge, build) - 1);
                    return 5 * baseValue * talentMultiplier;

                default: return base.Calculate(property, build, hitType, enemy);
            }
        }

        public override double CalculateStat(StatType statType, Build build)
        {
            var stat = base.CalculateStat(statType, build);

            switch (statType)
            {
                case StatType.ElectroDmgBonus:
                    stat += .4 * Math.Max(0, GetStat(StatType.EnergyRecharge, build) - 1);
                    break;
                case StatType.DefShred when Constellation >= 2:
                    stat += .6;
                    break;
            }

            return stat;
        }
    }
}
