using System.Collections.Generic;

namespace ShogunOptimizer
{
    public abstract class ArtifactSource
    {
        public readonly List<Artifact> Flowers = new();
        public readonly List<Artifact> Plumes = new();
        public readonly List<Artifact> Sands = new();
        public readonly List<Artifact> Goblets = new();
        public readonly List<Artifact> Circlets = new();
    }
}