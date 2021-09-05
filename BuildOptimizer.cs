using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShogunOptimizer
{
    public class BuildOptimizer
    {
        public Build GenerateBuilds(
            IEnumerable<Weapon> weapons, IEnumerable<Artifact> flowers, IEnumerable<Artifact> plumes, IEnumerable<Artifact> sands, IEnumerable<Artifact> goblets, IEnumerable<Artifact> circlets,
            Func<Build, double> evaluateBuild, Func<Build, bool> filterBuild = null)
        {
            var builds = new List<Build>();

            Console.WriteLine($" - Creating builds");

            foreach (var w in weapons)
                foreach (var f in flowers)
                    foreach (var p in plumes)
                        foreach (var s in sands)
                            foreach (var g in goblets)
                                foreach (var c in circlets)
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
