using ShogunOptimizer.BuildTargets;
using ShogunOptimizer.Importers;
using System;

namespace ShogunOptimizer
{
    public class Program
    {
        static void Main(string[] args)
        {
            BuildTarget buildTarget = new RaidenTarget();

            buildTarget.Initialize(out var character, out var enemy, out var weapons);

            Console.WriteLine($"Importing Artifacts...");

            var importer = new GoImporter(buildTarget.UpgradeArtifactsToLvl20);
            importer.Import("../../Debug/net5.0/godata.json", buildTarget.EquippedTo, buildTarget.AllowUnequipped);

            buildTarget.FilterArtifacts(importer.Flowers, importer.Plumes, importer.Sands, importer.Goblets, importer.Circlets);

            Console.WriteLine($"Evaluating {weapons.Count * importer.Flowers.Count * importer.Plumes.Count * importer.Sands.Count * importer.Goblets.Count * importer.Circlets.Count} Builds...");

            var optimizer = new BuildOptimizer();
            var build = optimizer.GenerateBuilds(weapons,
                importer.Flowers, importer.Plumes, importer.Sands, importer.Goblets, importer.Circlets,
                b => buildTarget.Evaluate(b, character, enemy));

            Console.Clear();

            Console.WriteLine();
            Console.WriteLine($"~~~ Build Value: {buildTarget.Evaluate(build, character, enemy):#.##} ~~~");
            Console.WriteLine();
            Console.WriteLine($"Max HP: {character.GetMaxHp(build):#}");
            Console.WriteLine($"ATK: {character.GetAtk(build):#}");
            Console.WriteLine($"DEF: {character.GetDef(build):#}");
            Console.WriteLine($"CRIT RATE: {character.GetStat(StatType.CritRate, build):P}");
            Console.WriteLine($"CRIT DMG: +{character.GetStat(StatType.CritDamage, build):P}");
            Console.WriteLine($"EM: {character.GetStat(StatType.ElementalMastery, build)}");
            Console.WriteLine($"ER: {character.GetStat(StatType.EnergyRecharge, build):P}");

            Console.WriteLine();
            buildTarget.DisplayResults(build, character, enemy);

            Console.WriteLine();
            Console.WriteLine($"~~~ {build.Weapon.GetType().Name} R{build.Weapon.Refine} ~~~");

            foreach (var artifact in build.Artifacts)
            {
                Console.WriteLine();
                Console.WriteLine($"~~~ {artifact.GetType().Name} / {artifact.Set.GetType().Name} ~~~");
                foreach ((var artifactStatType, var artifactStatValue) in artifact.Stats)
                    Console.WriteLine($"{artifactStatType}: {artifactStatValue}");
            };

        }
    }
}
