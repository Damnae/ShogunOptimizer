using ShogunOptimizer.ArtifactSources;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace ShogunOptimizer
{
    public class BuildOptimizer
    {
        public Build FindBestBuild(
            ICollection<Weapon> weapons, ArtifactSource artifactSource, ReadOnlyDictionary<Type, object> configs,
            Func<Build, double> evaluateBuild, Func<Build, bool> filterBuild = null)
        {
            Console.WriteLine($" - Creating {weapons.Count * artifactSource.Flowers.Count * artifactSource.Plumes.Count * artifactSource.Sands.Count * artifactSource.Goblets.Count * artifactSource.Circlets.Count} builds");

            var builds = new List<Build>();
            foreach (var w in weapons)
                foreach (var f in artifactSource.Flowers)
                    foreach (var p in artifactSource.Plumes)
                        foreach (var s in artifactSource.Sands)
                            foreach (var g in artifactSource.Goblets)
                                foreach (var c in artifactSource.Circlets)
                                    builds.Add(new Build(w, f, p, s, g, c, configs));

            Console.WriteLine($" - Evaluating builds");

#if DEBUG
            foreach (var build in builds)
                build.Value = evaluateBuild(build);
#else
            Parallel.For(0, builds.Count, index =>
            {
                var build = builds[index];

                if (filterBuild == null || filterBuild(build))
                    build.Value = evaluateBuild(build);
            });
#endif

            Console.WriteLine($" - Ranking builds");

            var bestBuild = (Build)null;
            foreach (var build in builds)
                if (bestBuild == null || bestBuild.Value < build.Value)
                    bestBuild = build;

            return bestBuild;
        }

        public Build FindIdealArtifacts(ICollection<Weapon> weapons, BuildTarget buildTarget, ReadOnlyDictionary<Type, object> configs, int generations, double rollFactor,
            Func<Build, double> evaluateBuild, Func<Build, bool> filterBuild = null)
        {
            var random = new Random();
            var bestValue = 0.0;
            var bestBuilds = Enumerable.Empty<Build>();

            for (var i = 0; i < generations; i++)
            {
                var artifactSource = buildTarget.GenerateArtifacts(rollFactor);

                var add = true;
                foreach (var b in bestBuilds)
                {
                    addAndMutate(b.Artifacts[0], artifactSource.Flowers, buildTarget, rollFactor, random, add);
                    addAndMutate(b.Artifacts[1], artifactSource.Plumes, buildTarget, rollFactor, random, add);
                    addAndMutate(b.Artifacts[2], artifactSource.Sands, buildTarget, rollFactor, random, add);
                    addAndMutate(b.Artifacts[3], artifactSource.Goblets, buildTarget, rollFactor, random, add);
                    addAndMutate(b.Artifacts[4], artifactSource.Circlets, buildTarget, rollFactor, random, add);
                    add = false;
                }

                Console.WriteLine($" - Creating {weapons.Count * artifactSource.Flowers.Count * artifactSource.Plumes.Count * artifactSource.Sands.Count * artifactSource.Goblets.Count * artifactSource.Circlets.Count} builds");

                var builds = new List<Build>();
                foreach (var w in weapons)
                    foreach (var f in artifactSource.Flowers)
                        foreach (var p in artifactSource.Plumes)
                            foreach (var s in artifactSource.Sands)
                                foreach (var g in artifactSource.Goblets)
                                    foreach (var c in artifactSource.Circlets)
                                        builds.Add(new Build(w, f, p, s, g, c, configs));

                Console.WriteLine($" - Evaluating builds");

#if DEBUG
                foreach (var build in builds)
                    build.Value = evaluateBuild(build);
#else
                Parallel.For(0, builds.Count, index =>
                {
                    var build = builds[index];

                    if (filterBuild == null || filterBuild(build))
                        build.Value = evaluateBuild(build);
                });
#endif

                bestBuilds = builds.OrderByDescending(b => b.Value).Take(5);

                var topValue = bestBuilds.First().Value;
                if (topValue > bestValue)
                {
                    Console.WriteLine($"      ({topValue:#.#})");
                    bestValue = topValue;
                }
            }

            return bestBuilds.First();
        }

        private static void addAndMutate(Artifact artifact, List<Artifact> artifacts, BuildTarget buildTarget, double rollFactor, Random random, bool add)
        {
            if (add)
                addUnique(artifact, artifacts);

            switch (random.Next(3))
            {
                case 0:
                    // Change set
                    {
                        var currentSet = artifact.Set.GetType();
                        var setType = buildTarget.UsefulSets.Except(Enumerable.Repeat(currentSet, 1)).PickOne(random, currentSet);
                        var set = (ArtifactSet)Activator.CreateInstance(setType);

                        addUnique(new Artifact
                        {
                            Set = set,
                            Stats = artifact.Stats,
                        }, artifacts);
                    }
                    break;

                case 1:
                case 2:
                    // Change 1 substat and reroll substats
                    {
                        var mainstat = artifact.Stats[0].Item1;
                        var substats = artifact.Stats.Skip(1).Select(s => s.Item1).ToArray();

                        if (random.NextDouble() > .5f)
                        {
                            var slot = random.Next(substats.Length);
                            substats[slot] = buildTarget.UsefulSubStats.Except(artifact.Stats.Select(s => s.Item1)).PickOne(random, substats[slot]);
                        }

                        addUnique(new Artifact
                        {
                            Set = artifact.Set,
                            Stats = ArtifactGenerator.GenerateStats(artifact.Stats[0].Item1, substats, rollFactor, random).ToArray(),
                        }, artifacts);
                    }
                    break;
            }
        }

        private static void addUnique(Artifact artifact, List<Artifact> artifacts)
        {
            if (!artifacts.Contains(artifact))
                artifacts.Add(artifact);
        }
    }
}
