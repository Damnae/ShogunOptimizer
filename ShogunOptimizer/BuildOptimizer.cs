using System;
using System.Collections.Generic;

namespace ShogunOptimizer
{
    public class BuildOptimizer
    {
        public Build GenerateBuilds<TCharacter>(TCharacter character, 
            IEnumerable<Weapon> weapons, IEnumerable<Artifact> flowers, IEnumerable<Artifact> plumes, IEnumerable<Artifact> sands, IEnumerable<Artifact> goblets, IEnumerable<Artifact> circlets,
            Enemy enemy, Func<Build, double> evaluateBuild)
        {
            var build = new Build();

            var bestBuild = (Build)null;
            var bestValue = double.MinValue;

            foreach (var w in weapons)
            {
                build.Weapon = w;
                foreach (var f in flowers)
                {
                    build.Flower = f;
                    foreach (var p in plumes)
                    {
                        build.Plume = p;
                        foreach (var s in sands)
                        {
                            build.Sands = s;
                            foreach (var g in goblets)
                            {
                                build.Goblet = g;
                                foreach (var c in circlets)
                                {
                                    build.Circlet = c;

                                    var newBuildValue = evaluateBuild(build);
                                    if (newBuildValue > bestValue)
                                    {
                                        bestValue = newBuildValue;
                                        bestBuild = build;

                                        build = new Build
                                        {
                                            Weapon = bestBuild.Weapon,
                                            Flower = bestBuild.Flower,
                                            Plume = bestBuild.Plume,
                                            Goblet = bestBuild.Goblet,
                                            Sands = bestBuild.Sands,
                                        };
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return bestBuild;
        }
    }
}
