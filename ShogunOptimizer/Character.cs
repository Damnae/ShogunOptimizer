﻿using System;
using System.Collections.Generic;

namespace ShogunOptimizer
{
    public class Character
    {
        public double Level = 90;
        public int Constellation = 0;

        public double BaseAtk;
        public double BaseHp;
        public double AscensionStat;
        public StatType AscensionStatType;

        public double[] Stats = new double[Enum.GetValues<StatType>().Length];

        public Character()
        {
            Stats[(int)StatType.CritRate] = .05;
            Stats[(int)StatType.CritDamage] = .50;
            Stats[(int)StatType.EnergyRecharge] = 1.00;
        }

        public virtual double GetBaseAtk(Build build) => BaseAtk + build.Weapon.BaseAtk;
        public virtual double GetAtk(Build build) => GetBaseAtk(build) * (1 + GetStat(StatType.AtkPercent, build)) + GetStat(StatType.AtkFlat, build);

        public virtual double GetMaxHp(Build build) => BaseHp * (1 + GetStat(StatType.HpPercent, build)) + GetStat(StatType.HpFlat, build);

        public virtual double GetDamage(Build build, DamageType damageType, Element element, HitType hitType, Enemy enemy)
        {
            var unmitigatedDamage = GetAtk(build) * GetMultiplier(build, damageType, element, hitType);

            var resMultiplier = 1 - enemy.Resistances[(int)element];
            var defMultiplier = (100 + Level) / ((100 + Level) + (100 + enemy.Level) * (1 - GetStat(StatType.DefShred, build)));

            return unmitigatedDamage * resMultiplier * defMultiplier;
        }

        public virtual double GetMultiplier(Build build, DamageType damageType, Element element, HitType hitType)
            => GetDmgMultiplier(build, damageType, element) + GetCritMultiplier(build, damageType, hitType);

        public virtual double GetDmgMultiplier(Build build, DamageType damageType, Element element)
        {
            var multiplier = 1
                + GetStat(StatType.DmgBonus, build)
                + GetStat(ElementToDmgBonus(element), build);

            switch (damageType)
            {
                case DamageType.Normal:
                    multiplier += GetStat(StatType.AttackDmgBonus, build);
                    break;
                case DamageType.Charged:
                    multiplier += GetStat(StatType.ChargedDmgBonus, build);
                    break;
                case DamageType.Plunge:
                    multiplier += GetStat(StatType.PlungeDmgBonus, build);
                    break;
                case DamageType.Skill:
                    multiplier += GetStat(StatType.SkillDmgBonus, build);
                    break;
                case DamageType.Burst:
                    multiplier += GetStat(StatType.BurstDmgBonus, build);
                    break;
            }
            return multiplier;
        }

        public virtual double GetCritMultiplier(Build build, DamageType damageType, HitType hitType)
        {
            if (hitType == HitType.Normal)
                return 1;

            var critRate = GetStat(StatType.CritRate, build);
            var critDamage = GetStat(StatType.CritDamage, build);

            switch (damageType)
            {
                case DamageType.Normal:
                    critRate += GetStat(StatType.AttackCritRateBonus, build);
                    critDamage += GetStat(StatType.AttackCritDamageBonus, build);
                    break;
                case DamageType.Charged:
                    critRate += GetStat(StatType.ChargedCritRateBonus, build);
                    critDamage += GetStat(StatType.ChargedCritDamageBonus, build);
                    break;
                case DamageType.Plunge:
                    critRate += GetStat(StatType.PlungeCritRateBonus, build);
                    critDamage += GetStat(StatType.PlungeCritDamageBonus, build);
                    break;
                case DamageType.Skill:
                    critRate += GetStat(StatType.SkillCritRateBonus, build);
                    critDamage += GetStat(StatType.SkillCritDamageBonus, build);
                    break;
                case DamageType.Burst:
                    critRate += GetStat(StatType.BurstCritRateBonus, build);
                    critDamage += GetStat(StatType.BurstCritDamageBonus, build);
                    break;
            }

            if (hitType == HitType.Critical)
                return 1 + critDamage;

            if (hitType == HitType.Averaged)
                return 1 + Math.Min(1.0, critRate) * critDamage;

            throw new InvalidOperationException($"Unknown hit type {hitType}");
        }

        public virtual double GetStat(StatType statType, Build build)
        {
            var stat = Stats[(int)statType];

            if (AscensionStatType == statType)
                stat += AscensionStat;

            foreach ((var weaponStatType, var weaponStatValue) in build.Weapon.Stats)
                if (weaponStatType == statType)
                    stat += weaponStatValue;

            stat += build.Weapon.GetStat(statType, build, this);

            var sets = new Dictionary<Type, int>();
            foreach (var artifact in build.Artifacts)
            {
                var setType = artifact.Set.GetType();
                if (sets.TryGetValue(setType, out var count))
                    sets[setType] = count + 1;
                else sets[setType] = 1;

                stat += artifact.Set.GetStat(statType, build, this, sets[setType]);

                foreach ((var artifactStatType, var artifactStatValue) in artifact.Stats)
                    if (artifactStatType == statType)
                        stat += artifactStatValue;
            }

            return stat;
        }

        public virtual double Calculate(string property, Build build, HitType hitType, Enemy enemy)
        {
            switch (property)
            {
            }
            return 0;
        }

        public static StatType ElementToDmgBonus(Element element) => element switch
        {
            Element.Physical => StatType.PhysicalDmgBonus,
            Element.Pyro => StatType.PyroDmgBonus,
            Element.Hydro => StatType.HydroDmgBonus,
            Element.Cryo => StatType.CryoDmgBonus,
            Element.Electro => StatType.ElectroDmgBonus,
            Element.Anemo => StatType.AnemoDmgBonus,
            Element.Geo => StatType.GeoDmgBonus,
            Element.Dendro => StatType.DendroDmgBonus,
            _ => throw new NotImplementedException(element.ToString()),
        };
    }
}
