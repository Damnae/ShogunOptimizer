using System;

namespace ShogunOptimizer
{
    public class Character
    {
        public double Level = 90;
        public int AttackLevel = 10;
        public int SkillLevel = 10;
        public int BurstLevel = 10;
        public int Constellation = 0;

        public double BaseHp;
        public double BaseAtk;
        public double BaseDef;

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
        public virtual double GetDef(Build build) => BaseDef * (1 + GetStat(StatType.DefPercent, build)) + GetStat(StatType.DefFlat, build);

        public virtual double GetMultiplier(Build build, DamageType damageType, Element element, HitType hitType, Enemy enemy)
        {
            var rawMultiplier = GetDmgMultiplier(build, damageType, element) * GetCritMultiplier(build, damageType, hitType);
            var reactionMultiplier = GetReactionMultiplier(GetReaction(element, enemy), build);

            var resMultiplier = 1 - enemy.Resistances[(int)element];
            var defMultiplier = (100 + Level) / ((100 + Level) + (100 + enemy.Level) * (1 - Math.Min(.9, GetStat(StatType.DefShred, build))));

            return rawMultiplier * resMultiplier * defMultiplier * reactionMultiplier;
        }

        public double GetReactionMultiplier(ElementalReaction reaction, Build build)
        {
            var multiplier = 1.0;
            var reactionBonus = 0.0;

            switch (reaction)
            {
                case ElementalReaction.Melt:
                    multiplier = 2.0;
                    reactionBonus += GetStat(StatType.MeltDmgBonus, build);
                    break;

                case ElementalReaction.Vaporize:
                    multiplier = 2.0;
                    reactionBonus += GetStat(StatType.VaporizeDmgBonus, build);
                    break;

                case ElementalReaction.ReverseMelt:
                    multiplier = 1.5;
                    reactionBonus += GetStat(StatType.MeltDmgBonus, build);
                    break;

                case ElementalReaction.ReverseVaporize:
                    multiplier = 1.5;
                    reactionBonus += GetStat(StatType.VaporizeDmgBonus, build);
                    break;

                default:
                    return multiplier;
            }

            var em = GetStat(StatType.ElementalMastery, build);
            var elementalMasteryBonus = 2.78 * em / (em + 1400);
            multiplier *= (1 + elementalMasteryBonus + reactionBonus);

            return multiplier;
        }

        public static ElementalReaction GetReaction(Element source, Enemy enemy)
        {
            return source switch
            {
                Element.Hydro when enemy.AffectedBy == Element.Pyro => ElementalReaction.Vaporize,
                Element.Pyro when enemy.AffectedBy == Element.Hydro => ElementalReaction.ReverseVaporize,
                Element.Pyro when enemy.AffectedBy == Element.Cryo => ElementalReaction.Melt,
                Element.Cryo when enemy.AffectedBy == Element.Pyro => ElementalReaction.ReverseMelt,
                Element.Pyro when enemy.AffectedBy == Element.Electro => ElementalReaction.Overloaded,
                Element.Electro when enemy.AffectedBy == Element.Pyro => ElementalReaction.Overloaded,
                Element.Hydro when enemy.AffectedBy == Element.Electro => ElementalReaction.ElectroCharged,
                Element.Electro when enemy.AffectedBy == Element.Hydro => ElementalReaction.ElectroCharged,
                Element.Cryo when enemy.AffectedBy == Element.Electro => ElementalReaction.Superconduct,
                Element.Electro when enemy.AffectedBy == Element.Cryo => ElementalReaction.Superconduct,
                Element.Pyro when enemy.AffectedBy == Element.Dendro => ElementalReaction.Burning,

                Element.Anemo when
                    enemy.AffectedBy == Element.Cryo ||
                    enemy.AffectedBy == Element.Electro ||
                    enemy.AffectedBy == Element.Hydro ||
                    enemy.AffectedBy == Element.Pyro 
                    => ElementalReaction.Swirl,

                _ => ElementalReaction.None,
            };
        }

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
                return 1 + Math.Max(0, Math.Min(1.0, critRate)) * critDamage;

            throw new InvalidOperationException($"Unknown hit type {hitType}");
        }

        public double GetStat(StatType statType, Build build) => build.GetCachedStat(statType, this);

        public virtual double CalculateStat(StatType statType, Build build)
        {
            var stat = Stats[(int)statType];

            if (AscensionStatType == statType)
                stat += AscensionStat;

            foreach ((var weaponStatType, var weaponStatValue) in build.Weapon.Stats)
                if (weaponStatType == statType)
                    stat += weaponStatValue;

            stat += build.Weapon.GetStat(statType, build, this);

            var setTypes = new Type[5];
            var setCount = new int[5];
            foreach (var artifact in build.Artifacts)
            {
                var setType = artifact.Set.GetType();

                for (var i = 0; i < setTypes.Length; i++)
                    if (setTypes[i] == null)
                    {
                        setTypes[i] = setType;
                        stat += artifact.Set.GetStat(statType, build, this, ++setCount[i]);
                        break;
                    }
                    else if (setTypes[i] == setType)
                    {
                        stat += artifact.Set.GetStat(statType, build, this, ++setCount[i]);
                        break;
                    }

                foreach ((var artifactStatType, var artifactStatValue) in artifact.Stats)
                    if (artifactStatType == statType)
                        stat += artifactStatValue;
            }

            return stat;
        }

        private readonly double[] AttackScalings = new[] { 1.0, 1.08, 1.16, 1.275, 1.35, 1.45, 1.575, 1.7, 1.825, 1.975, 2.125 };
        private readonly double[] PercentageScalings = new[] { 1.0, 1.075, 1.15, 1.25, 1.325, 1.4, 1.5, 1.6, 1.7, 1.8, 1.9, 2, 2.125 };
        private readonly double[] FlatScalings = new[] { 1.0, 1.1, 1.2, 1.325, 1.45, 1.575, 1.725, 1.875, 202.5, 2.2, 2.375, 2.55, 2.75 };

        public virtual double GetTalentAttackScaling(int level) => AttackScalings[level - 1];
        public virtual double GetTalentPercentageScaling(int level) => PercentageScalings[level - 1];
        public virtual double GetTalentFlatScaling(int level) => FlatScalings[level - 1];

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
