using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShogunOptimizer
{
    public class ArtifactGenerator : ArtifactSource
    {
        private static readonly StatType[] flowerMainStats = { StatType.HpFlat, };
        private static readonly StatType[] plumeMainStats = { StatType.AtkFlat, };
        private static readonly StatType[] sandsMainStats = { StatType.AtkFlat, StatType.DefPercent, StatType.HpPercent, StatType.EnergyRecharge, StatType.ElementalMastery, };
        private static readonly StatType[] gobletMainStats = { StatType.AtkFlat, StatType.DefPercent, StatType.HpPercent, StatType.EnergyRecharge, StatType.ElementalMastery,
            StatType.PhysicalDmgBonus, StatType.PyroDmgBonus, StatType.CryoDmgBonus, StatType.HydroDmgBonus, StatType.ElectroDmgBonus, StatType.AnemoDmgBonus, StatType.GeoDmgBonus, StatType.DendroDmgBonus, };
        private static readonly StatType[] circletMainStats = { StatType.AtkFlat, StatType.DefPercent, StatType.HpPercent, StatType.EnergyRecharge, StatType.ElementalMastery, StatType.CritRate, StatType.CritDamage, StatType.HealBonus, };

        public Type[] Sets;
        public StatType[] SandsMainStats;
        public StatType[] GobletMainStats;
        public StatType[] CircletMainStats;
        public StatType[] SubStats;

        public StatType[] MainStats
        {
            set
            {
                SandsMainStats = value.Intersect(sandsMainStats).ToArray();
                GobletMainStats = value.Intersect(gobletMainStats).ToArray();
                CircletMainStats = value.Intersect(circletMainStats).ToArray();
            }
        }

        public void Generate()
        {
#if DEBUG
            generate(Flowers, flowerMainStats, amount);
            generate(Plumes, plumeMainStats, amount);
            generate(Sands, SandsMainStats, amount);
            generate(Goblets, GobletMainStats, amount);
            generate(Circlets, CircletMainStats, amount);
#else
            Parallel.Invoke(
                () => generate(Flowers, flowerMainStats),
                () => generate(Plumes, plumeMainStats),
                () => generate(Sands, SandsMainStats),
                () => generate(Goblets, GobletMainStats),
                () => generate(Circlets, CircletMainStats)
            );
#endif
        }

        private void generate(List<Artifact> artifacts, StatType[] mainStats)
        {
            var random = new Random();
            foreach (var mainStat in mainStats)
            {
                var stats = new List<Tuple<StatType, double>>(5)
                    {
                        new(mainStat, GetMainStatLvl20(mainStat))
                    };

                // Fill Substats
                for (var i = 0; i < 4; i++)
                {
                    var availableSubstat = SubStats.Except(stats.Select(s => s.Item1)).ToArray();
                    if (availableSubstat.Length == 0)
                        break;

                    var substat = availableSubstat[random.Next(availableSubstat.Length)];
                    stats.Add(new(substat, GetSubStatRolls(substat).PickOne(random)));
                }

                // Increase Substats
                for (var i = 0; i < 5; i++)
                {
                    var slot = random.Next(1, stats.Count);

                    var substat = stats[slot];
                    stats[slot] = new(substat.Item1, substat.Item2 + GetSubStatRolls(substat.Item1).PickOne(random));
                }

                // Create for every set
                var statsArray = stats.ToArray();
                foreach (var set in Sets.Select(t => (ArtifactSet)Activator.CreateInstance(t)))
                    artifacts.Add(new Artifact
                    {
                        Set = set,
                        Stats = statsArray,
                    });
            }
        }

        private static double GetMainStatLvl20(StatType mainStat) =>
            mainStat switch
            {
                StatType.AtkFlat => 311,
                StatType.HpFlat => 4780,
                StatType.ElementalMastery => 187,
                StatType.AtkPercent => .466,
                StatType.DefPercent => .583,
                StatType.HpPercent => .466,
                StatType.CritRate => .311,
                StatType.CritDamage => .622,
                StatType.EnergyRecharge => .518,
                StatType.HealBonus => .359,
                StatType.PhysicalDmgBonus => .583,

                StatType.PyroDmgBonus
                or StatType.HydroDmgBonus
                or StatType.CryoDmgBonus
                or StatType.ElectroDmgBonus
                or StatType.AnemoDmgBonus
                or StatType.GeoDmgBonus
                or StatType.DendroDmgBonus
                    => .466,

                _ => throw new NotSupportedException($"Unknown main stat type {mainStat}"),
            };

        private static readonly double[] atkFlatRolls = { 14, 16, 18, 19, };
        private static readonly double[] hpFlatRolls = { 209, 239, 269, 299, };
        private static readonly double[] defFlatRolls = { 16, 19, 21, 23, };
        private static readonly double[] emRolls = { 16, 19, 21, 23, };
        private static readonly double[] atkPercentRolls = { .041, .047, .053, .058, };
        private static readonly double[] hpPercentRolls = { .041, .047, .053, .058, };
        private static readonly double[] defPercentRolls = { .051, .058, .066, .073, };
        private static readonly double[] critRateRolls = { .027, .031, .035, .039, };
        private static readonly double[] critDamageRolls = { .054, .062, .07, .078, };
        private static readonly double[] energyRegenRolls = { .045, .052, .058, .065, };

        private static double[] GetSubStatRolls(StatType stat) =>
            stat switch
            {
                StatType.AtkFlat => atkFlatRolls,
                StatType.HpFlat => hpFlatRolls,
                StatType.DefFlat => defFlatRolls,
                StatType.ElementalMastery => emRolls,
                StatType.AtkPercent => atkPercentRolls,
                StatType.DefPercent => defPercentRolls,
                StatType.HpPercent => hpPercentRolls,
                StatType.CritRate => critRateRolls,
                StatType.CritDamage => critDamageRolls,
                StatType.EnergyRecharge => energyRegenRolls,

                _ => throw new NotSupportedException($"Unknown sub stat type {stat}"),
            };
    }
}