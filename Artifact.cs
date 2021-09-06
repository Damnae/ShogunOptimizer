using System;
using System.Linq;

namespace ShogunOptimizer
{
    public class Artifact : IEquatable<Artifact>
    {
        public ArtifactSet Set;
        public Tuple<StatType, double>[] Stats;

        public bool Equals(Artifact other)
        {
            if (Set.GetType() != other.GetType())
                return false;

            if (Stats.Length != other.Stats.Length)
                return false;

            foreach (var stat in Stats)
                if (!other.Stats.Contains(stat))
                    return false;

            return true;
        }

        public override bool Equals(object obj) => Equals(obj as Artifact);
        public override int GetHashCode() => HashCode.Combine(Set, Stats);
    }
}
