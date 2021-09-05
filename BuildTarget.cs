using ShogunOptimizer.ArtifactSources;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShogunOptimizer
{
    public abstract class BuildTarget
    {
        public abstract bool UpgradeArtifactsToLvl20 { get; }
        public abstract string EquippedTo { get; }
        public abstract bool AllowUnequipped { get; }

        public abstract Type[] UsefulSets { get; }
        public abstract StatType[] UsefulMainStats { get; }
        public abstract StatType[] UsefulSubStats { get; }

        public abstract void Initialize(out Character character, out Enemy enemy, out ICollection<Weapon> weapons);

        public void FilterArtifacts(ArtifactSource artifactSource)
        {
            artifactSource.Sands.RemoveAll(p => !UsefulMainStats.Contains(p.Stats[0].Item1));
            artifactSource.Goblets.RemoveAll(p => !UsefulMainStats.Contains(p.Stats[0].Item1));
            artifactSource.Circlets.RemoveAll(p => !UsefulMainStats.Contains(p.Stats[0].Item1));
        }

        public abstract bool FilterBuild(Build build, Character character, Enemy enemy);
        public abstract double Evaluate(Build build, Character character, Enemy enemy);
        public abstract void DisplayResults(Build build, Character character, Enemy enemy);

        public virtual ArtifactSource ImportArtifacts()
        {
            var importer = new GoImporter(UpgradeArtifactsToLvl20);
            importer.Import("../../Debug/net5.0/godata.json", EquippedTo, AllowUnequipped);
            FilterArtifacts(importer);
            return importer;
        }

        public virtual ArtifactSource GenerateArtifacts()
        {
            var generator = new ArtifactGenerator
            {
                Sets = UsefulSets,
                MainStats = UsefulMainStats,
                SubStats = UsefulSubStats,
            };
            generator.Generate();
            return generator;
        }
    }
}
