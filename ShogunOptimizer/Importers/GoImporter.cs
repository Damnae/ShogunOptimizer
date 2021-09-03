using ShogunOptimizer.ArtifactSets;
using System;
using System.Collections.Generic;
using Tiny;

namespace ShogunOptimizer.Importers
{
    public class GoImporter
    {
        public readonly List<Artifact> Flowers = new();
        public readonly List<Artifact> Plumes = new();
        public readonly List<Artifact> Sands = new();
        public readonly List<Artifact> Goblets = new();
        public readonly List<Artifact> Circlets = new();

        public void Import(string path)
        {
            var data = TinyToken.Read(path);
            var artifactDatabase = data.Value<TinyObject>("artifactDatabase");

            foreach ((_, var artifactData) in artifactDatabase)
            {
                var level = artifactData.Value<int>("level");
                var setKey = artifactData.Value<string>("setKey");
                if (level < 20 || setKey == "EmblemOfSeveredFate")
                    continue;

                level = 20;

                var slotkey = artifactData.Value<string>("slotKey");
                var mainStatKey = artifactData.Value<string>("mainStatKey");

                var artifact = new Artifact();

                switch (slotkey)
                {
                    case "flower": Flowers.Add(artifact); break;
                    case "plume": Plumes.Add(artifact); break;
                    case "sands": Sands.Add(artifact); break;
                    case "goblet": Goblets.Add(artifact); break;
                    case "circlet": Circlets.Add(artifact); break;
                    default: throw new NotSupportedException(slotkey);
                }

                // Sets

                artifact.Set = setKey switch
                {
                    "EmblemOfSeveredFate" => new EmblemOfSeveredFate(),
                    _ => new ArtifactSet(),
                };

                // Stats

                var artifactStats = new List<Tuple<StatType, double>>();

                var levelFactor = level / 20.0;

                var mainstat = StatKeyToStatType(mainStatKey);
                switch (mainstat)
                {
                    case StatType.AtkFlat: artifactStats.Add(new(mainstat, 47 + (311 - 47) * levelFactor)); break;
                    case StatType.HpFlat: artifactStats.Add(new(mainstat, 717 + (4780 - 717) * levelFactor)); break;
                    case StatType.ElementalMastery: artifactStats.Add(new(mainstat, 28 + (187 - 28) * levelFactor)); break;
                    case StatType.AtkPercent: artifactStats.Add(new(mainstat, .07 + (.466 - .07) * levelFactor)); break;
                    case StatType.DefPercent: artifactStats.Add(new(mainstat, .087 + (.583 - .087) * levelFactor)); break;
                    case StatType.HpPercent: artifactStats.Add(new(mainstat, .07 + (.466 - .07) * levelFactor)); break;
                    case StatType.CritRate: artifactStats.Add(new(mainstat, .047 + (.311 - .047) * levelFactor)); break;
                    case StatType.CritDamage: artifactStats.Add(new(mainstat, .093 + (.622 - .093) * levelFactor)); break;
                    case StatType.EnergyRecharge: artifactStats.Add(new(mainstat, .078 + (.518 - .078) * levelFactor)); break;
                    case StatType.HealBonus: artifactStats.Add(new(mainstat, .054 + (.359 - .054) * levelFactor)); break;

                    case StatType.PhysicalDmgBonus:
                        artifactStats.Add(new(mainstat, .087 + (.583 - .087) * levelFactor)); 
                        break;

                    case StatType.PyroDmgBonus:
                    case StatType.HydroDmgBonus:
                    case StatType.CryoDmgBonus:
                    case StatType.ElectroDmgBonus:
                    case StatType.AnemoDmgBonus:
                    case StatType.GeoDmgBonus:
                    case StatType.DendroDmgBonus:
                        artifactStats.Add(new(mainstat, .07 + (.466 - .07) * levelFactor)); 
                        break;

                    case StatType.DefFlat:
                    default:
                        throw new NotSupportedException($"Unknown main stat type {mainstat}");
                }

                foreach (var substat in artifactData.Values<TinyObject>("substats"))
                {
                    var substatKey = substat.Value<string>("key");

                    if (string.IsNullOrEmpty(substatKey))
                        continue;

                    var substatValue = substat.Value<double>("value");

                    var statType = StatKeyToStatType(substatKey);
                    switch (statType)
                    {
                        case StatType.AtkFlat:
                        case StatType.DefFlat:
                        case StatType.HpFlat:
                        case StatType.ElementalMastery:
                            artifactStats.Add(new(statType, substatValue));
                            break;
                        case StatType.AtkPercent:
                        case StatType.DefPercent:
                        case StatType.HpPercent:
                        case StatType.CritRate:
                        case StatType.CritDamage:
                        case StatType.EnergyRecharge:
                            artifactStats.Add(new(statType, substatValue * .01));
                            break;
                        default:
                            throw new NotSupportedException($"Unknown stat type {statType}");
                    }
                }

                artifact.Stats = artifactStats.ToArray();
            }
        }

        public StatType StatKeyToStatType(string key) => key switch
        {
            "atk" => StatType.AtkFlat,
            "atk_" => StatType.AtkPercent,
            "def" => StatType.DefFlat,
            "def_" => StatType.DefPercent,
            "hp" => StatType.HpFlat,
            "hp_" => StatType.HpPercent,
            "critRate_" => StatType.CritRate,
            "critDMG_" => StatType.CritDamage,
            "enerRech_" => StatType.EnergyRecharge,
            "eleMas" => StatType.ElementalMastery,

            "physical_dmg_" => StatType.PhysicalDmgBonus,
            "pyro_dmg_" => StatType.PyroDmgBonus,
            "hydro_dmg_" => StatType.HydroDmgBonus,
            "cryo_dmg_" => StatType.CryoDmgBonus,
            "electro_dmg_" => StatType.ElectroDmgBonus,
            "anemo_dmg_" => StatType.AnemoDmgBonus,
            "geo_dmg_" => StatType.GeoDmgBonus,
            "dendro_dmg_" => StatType.DendroDmgBonus,

            "heal_" => StatType.HealBonus,

            _ => throw new NotSupportedException($"Unknown substat key {key}"),
        };
    }
}
