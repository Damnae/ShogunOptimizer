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
                Constellation = 0,
            };

            character.Stats[(int)StatType.AtkFlat] += 1000; // Bennett

            var weapons = new Weapon[]
            {
                new FavoniusLance(),
                new TheCatch(),
                new EngulfingLightning(),
            };

            var importer = new GoImporter();
            importer.Import("../../Debug/net5.0/godata.json");

            var enemy = new Enemy();

            Console.WriteLine($"Evaluating {weapons.Length * importer.Flowers.Count * importer.Plumes.Count * importer.Sands.Count * importer.Goblets.Count * importer.Circlets.Count} Builds...");

            double evaluateBuild(Build b) => character.Calculate(Raiden.PropertyBurstInitial, b, HitType.Averaged, enemy);
            var build = new BuildOptimizer().GenerateBuilds(character, weapons, importer.Flowers, importer.Plumes, importer.Sands, importer.Goblets, importer.Circlets,
                enemy, evaluateBuild);

            Console.Clear();

            Console.WriteLine($"~~~ Build Value: {evaluateBuild(build):#.##} (Crit: {character.Calculate(Raiden.PropertyBurstInitial, build, HitType.Critical, enemy):#.##}) ~~~");
            Console.WriteLine();
            Console.WriteLine($"ATK: {character.GetAtk(build):#.##}");
            Console.WriteLine($"CRIT RATE: {character.GetStat(StatType.CritRate, build):P}");
            Console.WriteLine($"CRIT DMG: +{character.GetStat(StatType.CritDamage, build):P}");
            Console.WriteLine($"EM: {character.GetStat(StatType.ElementalMastery, build)}");
            Console.WriteLine($"ER: {character.GetStat(StatType.EnergyRecharge, build):P}");
            Console.WriteLine();
            Console.WriteLine($"Electro DMG Bonus: {character.GetStat(StatType.ElectroDmgBonus, build):P}");
            Console.WriteLine($"Avg Burst Crit Multiplier: {character.GetCritMultiplier(build, DamageType.Burst, HitType.Averaged):P}");

            Console.WriteLine();
            Console.WriteLine($"~~~ {build.Weapon.GetType().Name} ~~~");

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
