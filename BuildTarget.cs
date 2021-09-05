using ShogunOptimizer.ArtifactSources;
using System.Collections.Generic;

namespace ShogunOptimizer
{
    public abstract class BuildTarget
    {
        public abstract bool UpgradeArtifactsToLvl20 { get; }
        public abstract string EquippedTo { get; }
        public abstract bool AllowUnequipped { get; }

        public abstract void Initialize(out Character character, out Enemy enemy, out ICollection<Weapon> weapons);
        public abstract void FilterArtifacts(ArtifactSource artifactSource);
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
    }
}
