using ShogunOptimizer.ArtifactSets;
using System;
using System.Collections.Generic;
using Tiny;

namespace ShogunOptimizer.ArtifactSources
{
    public class GoImporter : ArtifactSource
    {
        private readonly bool upgradeToLvl20;

        public GoImporter(bool upgradeToLvl20)
        {
            this.upgradeToLvl20 = upgradeToLvl20;
        }

        public void Import(string path, string equippedTo = null, bool allowUnequipped = true)
        {
            var data = TinyToken.Read(path);
            var artifactDatabase = data.Value<TinyObject>("artifactDatabase");

            foreach ((_, var artifactData) in artifactDatabase)
            {
                var location = artifactData.Value<string>("location");
                if (equippedTo != null && !(location == equippedTo || allowUnequipped && string.IsNullOrEmpty(location)))
                    continue;

                var level = artifactData.Value<int>("level");
               
                if (level < 20 && !upgradeToLvl20)
                    continue;
                level = 20;

                var artifact = new Artifact();

                // Sets

                var setKey = artifactData.Value<string>("setKey");
                artifact.Set = setKey switch
                {
                    "EmblemOfSeveredFate" => new EmblemOfSeveredFate(),
                    "ShimenawasReminiscence" => new ShimenawasReminiscence(),
                    "GladiatorsFinale" => new GladiatorsFinale(),
                    "NoblesseOblige" => new NoblesseOblige(),
                    "Thundersoother" => new Thundersoother(),
                    "ThunderingFury" => new ThunderingFury(),
                    "HeartOfDepth" => new HeartOfDepth(),
                    "MaidenBeloved" => new MaidenBeloved(),
                    "WanderersTroupe" => new WanderersTroupe(),
                    "TenacityOfTheMillelith" => new TenacityOfTheMillelith(),
                    _ => new MissingSet(setKey),
                };

                // Stats

                var artifactStats = new List<Tuple<StatType, double>>();

                var mainStatKey = artifactData.Value<string>("mainStatKey");
                var mainstat = StatKeyToStatType(mainStatKey);
                var levelFactor = level / 20.0;
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
                            throw new NotSupportedException($"Unknown substat type {statType}");
                    }
                }

                artifact.Stats = artifactStats.ToArray();

                var slotkey = artifactData.Value<string>("slotKey");
                switch (slotkey)
                {
                    case "flower": Flowers.Add(artifact); break;
                    case "plume": Plumes.Add(artifact); break;
                    case "sands": Sands.Add(artifact); break;
                    case "goblet": Goblets.Add(artifact); break;
                    case "circlet": Circlets.Add(artifact); break;
                    default: throw new NotSupportedException(slotkey);
                }
            }
        }

        public static StatType StatKeyToStatType(string key) => key switch
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
