using ShogunOptimizer.Characters;
using ShogunOptimizer.Importers;
using ShogunOptimizer.Weapons;
using System;

namespace ShogunOptimizer
{
    public class Program
    {
        static void Main(string[] args)
        {
            var character = new Raiden
            {
                Level = 90,
                AttackLevel = 10,
                SkillLevel = 10,
                BurstLevel = 10,
                Constellation = 0,

                Resolve = 60,
            };

            character.Stats[(int)StatType.BurstDmgBonus] += .003 * 90; // Raiden's E

            //character.Stats[(int)StatType.AtkPercent] += .25; // Pyro resonance

            //character.Stats[(int)StatType.AtkFlat] += 1000; // Bennett
            //character.Stats[(int)StatType.AtkPercent] += .2; // Bennett's 4 pieces NO

            var weapons = new Weapon[]
            {
                new TheCatch(5),
                new StaffOfHoma(1) { Under50PercentHp = true, },
                new EngulfingLightning(1),
            };

            var enemy = new Enemy { Level = 80, };

            var importer = new GoImporter(upgradeToLvl20: true);
            importer.Import("../../Debug/net5.0/godata.json"); //, "raidenshogun");

            importer.Sands.RemoveAll(p => !(p.Stats[0].Item1 == StatType.AtkPercent || p.Stats[0].Item1 == StatType.EnergyRecharge));
            importer.Goblets.RemoveAll(p => !(p.Stats[0].Item1 == StatType.AtkPercent || p.Stats[0].Item1 == StatType.ElectroDmgBonus));
            importer.Circlets.RemoveAll(p => !(p.Stats[0].Item1 == StatType.AtkPercent || p.Stats[0].Item1 == StatType.CritRate || p.Stats[0].Item1 == StatType.CritDamage));

            Console.WriteLine($"Evaluating {weapons.Length * importer.Flowers.Count * importer.Plumes.Count * importer.Sands.Count * importer.Goblets.Count * importer.Circlets.Count} Builds...");

            double evaluateBuild(Build b) =>
                character.Calculate(Raiden.PropertySkillInitial, b, HitType.Averaged, enemy) * 2
                + character.Calculate(Raiden.PropertySkillTick, b, HitType.Averaged, enemy) * 20
                + character.Calculate(Raiden.PropertyBurstInitial, b, HitType.Averaged, enemy)
                + character.Calculate(Raiden.PropertyBurst2N4C1N2C, b, HitType.Averaged, enemy);

            var build = new BuildOptimizer().GenerateBuilds(character, weapons, importer.Flowers, importer.Plumes, importer.Sands, importer.Goblets, importer.Circlets, evaluateBuild);

            Console.Clear();

            Console.WriteLine($"~~~ Build Value: {evaluateBuild(build):#.##} ~~~");
            Console.WriteLine();
            Console.WriteLine($"Max HP: {character.GetMaxHp(build):#}");
            Console.WriteLine($"ATK: {character.GetAtk(build):#.##}");
            Console.WriteLine($"CRIT RATE: {character.GetStat(StatType.CritRate, build):P}");
            Console.WriteLine($"CRIT DMG: +{character.GetStat(StatType.CritDamage, build):P}");
            Console.WriteLine($"EM: {character.GetStat(StatType.ElementalMastery, build)}");
            Console.WriteLine($"ER: {character.GetStat(StatType.EnergyRecharge, build):P}");
            Console.WriteLine();
            Console.WriteLine($"Electro DMG Bonus: {character.GetStat(StatType.ElectroDmgBonus, build):P}");
            Console.WriteLine($"Avg Burst Crit Multiplier: {character.GetCritMultiplier(build, DamageType.Burst, HitType.Averaged):P}");
            Console.WriteLine();
            Console.WriteLine($"Q Initial Damage: {character.Calculate(Raiden.PropertyBurstInitial, build, HitType.Normal, enemy):#} - {character.Calculate(Raiden.PropertyBurstInitial, build, HitType.Critical, enemy):#} (Avg {character.Calculate(Raiden.PropertyBurstInitial, build, HitType.Averaged, enemy):#})");
            Console.WriteLine($"Q Avg Attack Damage: {character.Calculate(Raiden.PropertyBurst2N4C1N2C, build, HitType.Averaged, enemy):#}");
            Console.WriteLine($"E Initial Damage: {character.Calculate(Raiden.PropertySkillInitial, build, HitType.Normal, enemy):#} - {character.Calculate(Raiden.PropertySkillInitial, build, HitType.Critical, enemy):#} (Avg {character.Calculate(Raiden.PropertySkillInitial, build, HitType.Averaged, enemy):#})");
            Console.WriteLine($"E Tick Damage: {character.Calculate(Raiden.PropertySkillTick, build, HitType.Normal, enemy):#} - {character.Calculate(Raiden.PropertySkillTick, build, HitType.Critical, enemy):#} (Avg {character.Calculate(Raiden.PropertySkillTick, build, HitType.Averaged, enemy):#})");
            Console.WriteLine($"E 20sec Avg Damage: {character.Calculate(Raiden.PropertySkillInitial, build, HitType.Averaged, enemy) * 2 + character.Calculate(Raiden.PropertySkillTick, build, HitType.Averaged, enemy) * 20:#}");
            Console.WriteLine();
            Console.WriteLine($"Q Energy Restored: {character.Calculate(Raiden.PropertyBurstEnergyRestored, build, HitType.Normal, enemy):#.##}");

            Console.WriteLine();
            Console.WriteLine($"~~~ {build.Weapon.GetType().Name} R{build.Weapon.Refine} ~~~");

            foreach (var artifact in build.Artifacts)
            {
                Console.WriteLine();
                Console.WriteLine($"~~~ {artifact.GetType().Name} / {artifact.Set.GetType().Name} ~~~");
                foreach ((var artifactStatType, var artifactStatValue) in artifact.Stats)
                    Console.WriteLine($"{artifactStatType}: {artifactStatValue}");
            }
        }
    }
}
