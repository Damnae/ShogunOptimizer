using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShogunOptimizer
{
    public class BuildOptimizer
    {
        public Build FindBestBuild(
            ICollection<Weapon> weapons, BuildTarget buildTarget,
            Func<Build, double> evaluateBuild, Func<Build, bool> filterBuild = null)
        {
            Console.WriteLine($" - Importing artifacts");

            var artifactSource = buildTarget.ImportArtifacts();

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
    }
}
