using ShogunOptimizer.BuildTargets;
using System;

namespace ShogunOptimizer
{
    public class Program
    {
        static void Main(string[] args)
        {
            BuildTarget buildTarget = new IttoTarget();

            buildTarget.Initialize(out var character, out var enemy, out var weapons);
            var configs = buildTarget.GetConfigs();

            var optimizer = new BuildOptimizer();

            var build = buildTarget.UseGeneratedArtifacts ?
                optimizer.FindIdealArtifacts(weapons, buildTarget, configs, 32, .5,
                    b => buildTarget.Evaluate(b, character, enemy),
                    b => buildTarget.FilterBuild(b, character, enemy))
                : optimizer.FindBestBuild(weapons, buildTarget.ImportArtifacts(), configs,
                    b => buildTarget.Evaluate(b, character, enemy),
                    b => buildTarget.FilterBuild(b, character, enemy));

            if (build == null || build.Value == 0)
            {
                Console.WriteLine();
                Console.WriteLine($"~~~ No build found ~~~");
                return;
            }

            Console.Clear();

            Console.WriteLine();
            Console.WriteLine($"~~~ Build Value: {build.Value:#.##} ~~~");
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
                Console.WriteLine(artifact);
            };
        }
    }
}
