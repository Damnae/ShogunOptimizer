using System.Collections.Generic;

namespace ShogunOptimizer
{
    public class Build
    {
        public Weapon Weapon;

        public Flower Flower;
        public Plume Plume;
        public Sands Sands;
        public Goblet Goblet;
        public Circlet Circlet;

        public IEnumerable<Artifact> Artifacts
        {
            get
            {
                yield return Flower;
                yield return Plume;
                yield return Sands;
                yield return Goblet;
                yield return Circlet;
            }
        }
    }
}
