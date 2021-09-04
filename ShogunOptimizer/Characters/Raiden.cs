using System;

namespace ShogunOptimizer.Characters
{
    public class Raiden : Character
    {
        public int Resolve = 60;

        public Raiden()
        {
            BaseAtk = 337;
            BaseHp = 12907;

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

        public const string PropertyBurst3N4C1N2C = "burst3N4C1N2C";
        
        public override double Calculate(string property, Build build, HitType hitType, Enemy enemy)
        {
            var burstScaling = GetTalentPercentageScaling(BurstLevel);
            switch (property)
            {
                case PropertySkillInitial: return (1.172 * GetTalentPercentageScaling(SkillLevel)) * GetDamage(build, DamageType.Skill, Element.Electro, hitType, enemy);
                case PropertySkillTick: return (0.42 * GetTalentPercentageScaling(SkillLevel)) * GetDamage(build, DamageType.Skill, Element.Electro, hitType, enemy);

                case PropertyBurstInitial: return (4.008 * burstScaling + 0.0389 * burstScaling * Resolve) * GetDamage(build, DamageType.Burst, Element.Electro, hitType, enemy);
                
                case PropertyBurstAttack1: return (.4474 * burstScaling + .0073 * burstScaling * Resolve) * GetDamage(build, DamageType.Burst, Element.Electro, hitType, enemy);
                case PropertyBurstAttack2: return (.4396 * burstScaling + .0073 * burstScaling * Resolve) * GetDamage(build, DamageType.Burst, Element.Electro, hitType, enemy);
                case PropertyBurstAttack3: return (.5382 * burstScaling + .0073 * burstScaling * Resolve) * GetDamage(build, DamageType.Burst, Element.Electro, hitType, enemy);
                case PropertyBurstAttack4A: return (.3089 * burstScaling + .0073 * burstScaling * Resolve) * GetDamage(build, DamageType.Burst, Element.Electro, hitType, enemy);
                case PropertyBurstAttack4B: return (.3098 * burstScaling + .0073 * burstScaling * Resolve) * GetDamage(build, DamageType.Burst, Element.Electro, hitType, enemy);
                case PropertyBurstAttack5: return (.7394 * burstScaling + .0073 * burstScaling * Resolve) * GetDamage(build, DamageType.Burst, Element.Electro, hitType, enemy);
                case PropertyBurstChargedA: return (.616 * burstScaling + .0073 * burstScaling * Resolve) * GetDamage(build, DamageType.Burst, Element.Electro, hitType, enemy);
                case PropertyBurstChargedB: return (.7436 * burstScaling + .0073 * burstScaling * Resolve) * GetDamage(build, DamageType.Burst, Element.Electro, hitType, enemy);

                case PropertyBurst3N4C1N2C:
                {
                    var damage = GetDamage(build, DamageType.Burst, Element.Electro, hitType, enemy);
                    return (.4474 * burstScaling + .0073 * burstScaling * Resolve) * damage * 5
                        + (.4396 * burstScaling + .0073 * burstScaling * Resolve) * damage * 5
                        + (.5382 * burstScaling + .0073 * burstScaling * Resolve) * damage * 4
                        + (.3089 * burstScaling + .0073 * burstScaling * Resolve) * damage * 4
                        + (.3098 * burstScaling + .0073 * burstScaling * Resolve) * damage * 4
                        + (.616 * burstScaling + .0073 * burstScaling * Resolve) * damage * 5
                        + (.7436 * burstScaling + .0073 * burstScaling * Resolve) * damage * 5;
                }

                case PropertyBurstEnergyRestored: return 5 * Math.Min(2.5, 1.6 + 0.1 * BurstLevel) * (1 + .006 * Math.Max(0, base.GetStat(StatType.EnergyRecharge, build) - 1));

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
