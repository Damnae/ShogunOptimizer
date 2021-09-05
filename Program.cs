using ShogunOptimizer.BuildTargets;
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

            var artifactSource = buildTarget.ImportArtifacts();

            Console.WriteLine($"Evaluating {weapons.Count * artifactSource.Flowers.Count * artifactSource.Plumes.Count * artifactSource.Sands.Count * artifactSource.Goblets.Count * artifactSource.Circlets.Count} Builds...");

            var optimizer = new BuildOptimizer();
            var build = optimizer.GenerateBuilds(weapons,
                artifactSource.Flowers, artifactSource.Plumes, artifactSource.Sands, artifactSource.Goblets, artifactSource.Circlets,
                b => buildTarget.Evaluate(b, character, enemy), b => buildTarget.FilterBuild(b, character, enemy));

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
                Console.WriteLine($"~~~ {artifact.Set} ~~~");
                foreach ((var artifactStatType, var artifactStatValue) in artifact.Stats)
                    Console.WriteLine($"{artifactStatType}: {artifactStatValue}");
            };
        }
    }
}
