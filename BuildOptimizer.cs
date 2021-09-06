using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShogunOptimizer
{
    public class BuildOptimizer
    {
        public Build FindBestBuild(
            ICollection<Weapon> weapons, ArtifactSource artifactSource,
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
                                    builds.Add(new Build(w, f, p, s, g, c));

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

        public Build FindIdealArtifacts(ICollection<Weapon> weapons, BuildTarget buildTarget, int generations,
            Func<Build, double> evaluateBuild, Func<Build, bool> filterBuild = null)
        {
            var random = new Random();
            var bestBuilds = Enumerable.Empty<Build>();

            for (var i = 0; i < generations; i++)
            {
                var artifactSource = buildTarget.GenerateArtifacts();

                foreach (var b in bestBuilds)
                {
                    addUnique(b.Artifacts[0], artifactSource.Flowers, buildTarget, random);
                    addUnique(b.Artifacts[1], artifactSource.Plumes, buildTarget, random);
                    addUnique(b.Artifacts[2], artifactSource.Sands, buildTarget, random);
                    addUnique(b.Artifacts[3], artifactSource.Goblets, buildTarget, random);
                    addUnique(b.Artifacts[4], artifactSource.Circlets, buildTarget, random);
                }

                Console.WriteLine($" - Creating {weapons.Count * artifactSource.Flowers.Count * artifactSource.Plumes.Count * artifactSource.Sands.Count * artifactSource.Goblets.Count * artifactSource.Circlets.Count} builds");

                var builds = new List<Build>();
                foreach (var w in weapons)
                    foreach (var f in artifactSource.Flowers)
                        foreach (var p in artifactSource.Plumes)
                            foreach (var s in artifactSource.Sands)
                                foreach (var g in artifactSource.Goblets)
                                    foreach (var c in artifactSource.Circlets)
                                        builds.Add(new Build(w, f, p, s, g, c));

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
                Console.WriteLine($"      ({bestBuilds.First().Value:#.#})");
            }

            return bestBuilds.First();
        }

        private void addUnique(Artifact artifact, List<Artifact> artifacts, BuildTarget buildTarget, Random random)
        {
            if (!artifacts.Contains(artifact))
                artifacts.Add(artifact);
        }
    }
}
