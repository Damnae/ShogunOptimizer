using System.Collections.Generic;

namespace ShogunOptimizer
{
    public abstract class BuildTarget
    {
        public abstract bool UpgradeArtifactsToLvl20 { get; }
        public abstract string EquippedTo { get; }
        public abstract bool AllowUnequipped { get; }

        public abstract void Initialize(out Character character, out Enemy enemy, out ICollection<Weapon> weapons);
        public abstract void FilterArtifacts(List<Artifact> flowers, List<Artifact> plumes, List<Artifact> sands, List<Artifact> goblets, List<Artifact> circlets);
        public abstract double Evaluate(Build build, Character character, Enemy enemy);
        public abstract void DisplayResults(Build build, Character character, Enemy enemy);
    }
}
