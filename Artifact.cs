using System;
using System.Linq;
using System.Text;

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

        public override string ToString()
        {
            var builder = new StringBuilder();

            builder.AppendLine($" - {Set} - ");
            foreach ((var statType, var statValue) in Stats)
                if (isFlat(statType))
                    builder.AppendLine($"{statType}: {statValue}");
                else
                    builder.AppendLine($"{statType}: {statValue:P}");

            return builder.ToString();
        }

        private static bool isFlat(StatType statType) => statType switch
        {
            StatType.AtkFlat or 
            StatType.DefFlat or 
            StatType.HpFlat or 
            StatType.ElementalMastery 
                => true,
            _ => false,
        };
    }
}
