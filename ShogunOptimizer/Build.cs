using System;

namespace ShogunOptimizer
{
    public class Build
    {
        public readonly Weapon Weapon;
        public readonly Artifact[] Artifacts;

        public double Value;

        public Build(Weapon weapon, Artifact flower, Artifact plume, Artifact sands, Artifact goblet, Artifact circlet)
        {
            Weapon = weapon;
            Artifacts = new[] { flower, plume, sands, goblet, circlet, };
        }

        #region Stat Cache

        private static readonly int cacheLength = Enum.GetValues<StatType>().Length;
        private readonly double[] cachedStats = new double[cacheLength];
        private readonly bool[] hasCachedStats = new bool[cacheLength];

        public double GetCachedStat(StatType statType, Character character)
        {
            double value;
            if (!hasCachedStats[(int)statType])
            {
                hasCachedStats[(int)statType] = true;
                cachedStats[(int)statType] = value = character.CalculateStat(statType, this);
            }
            else value = cachedStats[(int)statType];

            return value;
        }

        #endregion
    }
}
